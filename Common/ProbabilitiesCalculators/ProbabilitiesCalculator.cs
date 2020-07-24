using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Cliquely
{
	public class ProbabilitiesCalculator
	{
		private readonly string[] EmptyChars = { " ", "\t" };

		protected Dictionary<uint, string[]> CleanedData { get; }
		public Dictionary<string, List<uint>> ReversedCleanedData { get; }
		public Dictionary<string, eTaxonomy> GenomeLine { get; }
		public float ProbabilityThreshold { get; }
		public eTaxonomy Taxonomy { get; }

		public ProbabilitiesCalculator(float probabilityThreshold, eTaxonomy taxonomy)
		{
			ProbabilityThreshold = probabilityThreshold;
			Taxonomy = taxonomy;

			CleanedData = GetCleanedData(ConfigurationManager.AppSettings["cleanedDataPath"]);
			ReversedCleanedData = GetReversedCleanedData(ConfigurationManager.AppSettings["reversedCleanedDataPath"]);
			GenomeLine = GetGenomeLine(ConfigurationManager.AppSettings["genomeLinePath"]);

			FilterCleanedDataBacteriaByTaxonomy();
		}

		private void FilterCleanedDataBacteriaByTaxonomy()
		{
			if (Taxonomy == eTaxonomy.All) return;

			var filteredGenes = new List<uint>();
			var filteredCleanedData = new Dictionary<uint, string[]>();

			foreach (var gene in CleanedData.Keys)
			{
				var filteredBacteria = CleanedData[gene].ToList().Where(
					bacteria => GenomeLine.ContainsKey(bacteria) && (GenomeLine[bacteria] & Taxonomy) != 0).ToArray();

				if (filteredBacteria.Any())
				{
					filteredCleanedData[gene] = filteredBacteria;
				}
				else
				{
					filteredGenes.Add(gene);
				}
			}

			filteredGenes.ForEach(gene => CleanedData.Remove(gene));
			filteredCleanedData.Keys.ToList().ForEach(gene => CleanedData[gene] = filteredCleanedData[gene]);
		}

		public virtual Dictionary<uint, Dictionary<uint, float>> GetProbabilities()
		{
			var probabilities = GetProbabilitiesNetwork(CleanedData);

			return probabilities.Count > 0 ? probabilities : null;
		}

		protected virtual Dictionary<uint, Dictionary<uint, float>> GetProbabilitiesNetwork(Dictionary<uint, string[]> bacteriaForNeighbours)
		{
			var probabilities = new Dictionary<uint, Dictionary<uint, float>>();
			var keysList = bacteriaForNeighbours.Keys.ToList();

			for (var i = 0; i < keysList.Count; i++)
			{
				var geneI = keysList[i];
				probabilities[geneI] = new Dictionary<uint, float>();

				for (var j = 0; j < i; j++)
				{
					var geneJ = keysList[j];
					probabilities[geneJ] = new Dictionary<uint, float>();

					var probability = CalculateGeneProbability(bacteriaForNeighbours[geneI], bacteriaForNeighbours[geneJ]);

					if (probability >= ProbabilityThreshold)
					{
						probabilities[geneI][geneJ] = probability;
						probabilities[geneJ][geneI] = probability;
					}
				}
			}

			return probabilities;
		}

		private Dictionary<string, List<uint>> GetReversedCleanedData(string reversedCleanedDataPath)
		{
			var reversedCleanedData = new Dictionary<string, List<uint>>();

			using (var fileReader = new StreamReader(reversedCleanedDataPath))
			{
				fileReader.ReadLine();
				fileReader.ReadLine();
				string line;

				while ((line = fileReader.ReadLine()) != null)
				{
					reversedCleanedData.Add(line.Split(EmptyChars, StringSplitOptions.RemoveEmptyEntries)[0],
						GetGenesForBacteria(line));
				}
			}

			return reversedCleanedData;
		}

		private Dictionary<uint, string[]> GetCleanedData(string cleanedDataPath)
		{
			var cleanedData = new Dictionary<uint, string[]>();

			using (var fileReader = new StreamReader(cleanedDataPath))
			{
				fileReader.ReadLine();
				fileReader.ReadLine();

				string line;

				while ((line = fileReader.ReadLine()) != null)
				{
					var gene = uint.Parse(line.Split(EmptyChars, StringSplitOptions.RemoveEmptyEntries)[0]);

					cleanedData.Add(gene, GetBacteriasForGene(line));
				}
			}

			return cleanedData;
		}

		private Dictionary<string, eTaxonomy> GetGenomeLine(string genomeLinePath)
		{
			var genomeLine = new Dictionary<string, eTaxonomy>();

			using (var fileReader = new StreamReader(genomeLinePath))
			{
				string line;

				while ((line = fileReader.ReadLine()) != null)
				{
					var lineArray = line.Split('\t');

					if (Enum.TryParse(lineArray[0], true, out eTaxonomy taxonomy))
					{
						genomeLine.Add(lineArray[1], taxonomy);
					}
				}
			}

			return genomeLine;
		}

		private string[] GetBacteriasForGene(string fileLine)
		{
			var splitedFileLine = fileLine.Split(EmptyChars, StringSplitOptions.RemoveEmptyEntries);
			var bacteriasForGene = splitedFileLine.Skip(2).ToArray();

			return bacteriasForGene;
		}

		private List<uint> GetGenesForBacteria(string fileLine)
		{
			var splitedFileLine = fileLine.Split(EmptyChars, StringSplitOptions.RemoveEmptyEntries);
			var bacteriasForGene = splitedFileLine.Skip(2).Select(uint.Parse).ToList();

			return bacteriasForGene;
		}

		protected float CalculateGeneProbability(IReadOnlyCollection<string> bacteriasForGene1, IReadOnlyCollection<string> bacteriasForGene2)
		{
			var amountOfIntersectedBacterias = IntersectSorted(bacteriasForGene1, bacteriasForGene2).Count();

			float numerator = amountOfIntersectedBacterias * amountOfIntersectedBacterias;
			float denominator = bacteriasForGene1.Count * bacteriasForGene2.Count;

			return numerator / denominator;
		}

		private IEnumerable<string> IntersectSorted(IEnumerable<string> sequence1, IEnumerable<string> sequence2)
		{
			using (var cursor1 = sequence1.GetEnumerator())
			using (var cursor2 = sequence2.GetEnumerator())
			{
				if (!cursor1.MoveNext() || !cursor2.MoveNext())
				{
					yield break;
				}
				var value1 = cursor1.Current;
				var value2 = cursor2.Current;

				while (true)
				{
					var comparison = value1.CompareTo(value2);

					if (comparison < 0)
					{
						if (!cursor1.MoveNext())
						{
							yield break;
						}
						value1 = cursor1.Current;
					}
					else if (comparison > 0)
					{
						if (!cursor2.MoveNext())
						{
							yield break;
						}
						value2 = cursor2.Current;
					}
					else
					{
						yield return value1;
						if (!cursor1.MoveNext() || !cursor2.MoveNext())
						{
							yield break;
						}
						value1 = cursor1.Current;
						value2 = cursor2.Current;
					}
				}
			}
		}
	}
}