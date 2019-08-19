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

		public Dictionary<uint, float> GeneNeighboursProbabilities { get; private set; }
		public Dictionary<string, List<uint>> ReversedCleanedData { get; }
		private Dictionary<uint, string[]> CleanedData { get; }
		private uint  SourceGene { get; }
		private float Probability { get; }

		public ProbabilitiesCalculator(uint sourceGene, float probability)
		{
			SourceGene = sourceGene;
			Probability = probability;

			CleanedData = GetCleanedData(ConfigurationManager.AppSettings["cleanedDataPath"]);
			ReversedCleanedData = GetReversedCleanedData(ConfigurationManager.AppSettings["reversedCleanedDataPath"]);
		}

		public Dictionary<uint, Dictionary<uint, float>> GetProbabilities()
		{
			var sourceGeneNeighbours = GetSourceGeneNeighbours();
			FilterInNotEnoughBacteria(sourceGeneNeighbours);

			var bacteriaForNeighbours = GetBacteriaForNeighbours(sourceGeneNeighbours);
			GeneNeighboursProbabilities = CalculateProbabilitiesWithGenes(SourceGene, bacteriaForNeighbours);
			FilterByThresholdProbability(bacteriaForNeighbours, sourceGeneNeighbours);

			bacteriaForNeighbours = bacteriaForNeighbours.Where(x => sourceGeneNeighbours.Keys.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

			var probabilities = GetProbabilitiesNetwork(bacteriaForNeighbours);

			return probabilities.Count > 0 ? probabilities : null;
		}

		private Dictionary<uint, Dictionary<uint, float>> GetProbabilitiesNetwork(Dictionary<uint, string[]> bacteriaForNeighbours)
		{
			var probabilities = new Dictionary<uint, Dictionary<uint, float>>();

			foreach (var gene in bacteriaForNeighbours.Keys)
			{
				var dic = CalculateProbabilitiesWithGenes(gene, bacteriaForNeighbours);

				if (dic.Count > 0)
				{
					probabilities.Add(gene, dic);
				}
			}

			return probabilities;
		}

		private void FilterByThresholdProbability(Dictionary<uint, string[]> bacteriaForNeighbours, Dictionary<uint, int> sourceGeneNeighbours)
		{
			var toDelete = new List<uint>();

			foreach (var gene in bacteriaForNeighbours.Keys)
			{
				if (!GeneNeighboursProbabilities.ContainsKey(gene))
				{
					toDelete.Add(gene);
				}
			}

			foreach (var gene in toDelete)
			{
				sourceGeneNeighbours.Remove(gene);
			}
		}

		private Dictionary<uint, string[]> GetBacteriaForNeighbours(Dictionary<uint, int> sourceGeneNeighbours)
		{
			var bacteriaForNeighbours = new Dictionary<uint, string[]>();

			foreach (var gene in sourceGeneNeighbours.Keys)
			{
				bacteriaForNeighbours.Add(gene, CleanedData[gene]);
			}

			return bacteriaForNeighbours;
		}

		private void FilterInNotEnoughBacteria(Dictionary<uint, int> sourceGeneNeighbours)
		{
			var toDelete = new List<uint>();

			foreach (var gene in sourceGeneNeighbours)
			{
				if (gene.Value < CleanedData[SourceGene].Length * Probability)
				{
					toDelete.Add(gene.Key);
				}
			}

			foreach (var gene in toDelete)
			{
				sourceGeneNeighbours.Remove(gene);
			}
		}

		private Dictionary<uint, int> GetSourceGeneNeighbours()
		{
			var sourceGeneNeighbours = new Dictionary<uint, int>();
			var bacteriasForGene = CleanedData[SourceGene];

			foreach (var bacteria in bacteriasForGene)
			{
				foreach (var gene in ReversedCleanedData[bacteria])
				{
					if (!sourceGeneNeighbours.ContainsKey(gene))
					{
						sourceGeneNeighbours.Add(gene, 1);
					}
					else
					{
						sourceGeneNeighbours[gene] = sourceGeneNeighbours[gene] + 1;
					}
				}
			}

			return sourceGeneNeighbours;
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
					cleanedData.Add(uint.Parse(line.Split(EmptyChars, StringSplitOptions.RemoveEmptyEntries)[0]),
						GetBacteriasForGene(line));
				}
			}

			return cleanedData;
		}

		private Dictionary<uint, float> CalculateProbabilitiesWithGenes(uint gene, Dictionary<uint, string[]> bacteriasForGene)
		{
			var probabilities = new Dictionary<uint, float>(bacteriasForGene.Count);

			foreach (var j in bacteriasForGene.Keys)
			{
				if (gene != j)
				{
					var probability = CalculateGeneProbability(bacteriasForGene[gene], bacteriasForGene[j]);

					if (probability >= Probability)
					{
						probabilities[j] = probability;
					}
				}
			}

			return probabilities;
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

		private float CalculateGeneProbability(IReadOnlyCollection<string> bacteriasForGene1, IReadOnlyCollection<string> bacteriasForGene2)
		{
			var amountOfIntersectedBacterias = bacteriasForGene1.Intersect(bacteriasForGene2).Count();

			float numerator = amountOfIntersectedBacterias * amountOfIntersectedBacterias;
			float denominator = bacteriasForGene1.Count * bacteriasForGene2.Count;

			return numerator / denominator;
		}
	}
}