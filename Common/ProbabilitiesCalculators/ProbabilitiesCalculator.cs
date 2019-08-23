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
		public float Probability { get; }

		public ProbabilitiesCalculator(float probability)
		{
			Probability = probability;

			CleanedData = GetCleanedData(ConfigurationManager.AppSettings["cleanedDataPath"]);
			ReversedCleanedData = GetReversedCleanedData(ConfigurationManager.AppSettings["reversedCleanedDataPath"]);
		}

		public virtual Dictionary<uint, Dictionary<uint, float>> GetProbabilities()
		{
			var probabilities = GetProbabilitiesNetwork(CleanedData);

			return probabilities.Count > 0 ? probabilities : null;
		}

		protected Dictionary<uint, Dictionary<uint, float>> GetProbabilitiesNetwork(Dictionary<uint, string[]> bacteriaForNeighbours)
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

		protected Dictionary<uint, float> CalculateProbabilitiesWithGenes(uint gene, Dictionary<uint, string[]> bacteriasForGene)
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

		protected float CalculateGeneProbability(IReadOnlyCollection<string> bacteriasForGene1, IReadOnlyCollection<string> bacteriasForGene2)
		{
			var amountOfIntersectedBacterias = bacteriasForGene1.Intersect(bacteriasForGene2).Count();

			float numerator = amountOfIntersectedBacterias * amountOfIntersectedBacterias;
			float denominator = bacteriasForGene1.Count * bacteriasForGene2.Count;

			return numerator / denominator;
		}
	}
}