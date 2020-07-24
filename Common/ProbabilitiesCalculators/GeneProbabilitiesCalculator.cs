using System.Collections.Generic;
using System.Linq;

namespace Cliquely
{
	public class GeneProbabilitiesCalculator : ProbabilitiesCalculator
	{
		public Dictionary<uint, float> GeneNeighboursProbabilities { get; private set; }

		private uint SourceGene { get; }

		public GeneProbabilitiesCalculator(uint sourceGene, float probabilityThreshold, eTaxonomy taxonomy) : base(probabilityThreshold, taxonomy)
		{
			SourceGene = sourceGene;
		}

		public override Dictionary<uint, Dictionary<uint, float>> GetProbabilities()
		{
			if (!CleanedData.ContainsKey(SourceGene)) return null;
			
			var sourceGeneNeighbours = GetSourceGeneNeighbours();
			FilterByOccurrenceInBacteriaThreshold(sourceGeneNeighbours);

			var bacteriaForNeighbours = GetBacteriaForNeighbours(sourceGeneNeighbours);
			GeneNeighboursProbabilities = CalculateProbabilitiesWithGenes(SourceGene, bacteriaForNeighbours);
			FilterByThresholdProbability(bacteriaForNeighbours, sourceGeneNeighbours);

			bacteriaForNeighbours = bacteriaForNeighbours.Where(x => sourceGeneNeighbours.Keys.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

			var probabilities = GetProbabilitiesNetwork(bacteriaForNeighbours);

			return probabilities.Count > 0 ? probabilities : null;
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

		private void FilterByOccurrenceInBacteriaThreshold(Dictionary<uint, int> sourceGeneNeighbours)
		{
			var toDelete = new List<uint>();

			foreach (var gene in sourceGeneNeighbours)
			{
				if (gene.Value < CleanedData[SourceGene].Length * ProbabilityThreshold)
				{
					toDelete.Add(gene.Key);
				}
			}

			toDelete.ForEach(gene => sourceGeneNeighbours.Remove(gene));
		}

		private Dictionary<uint, string[]> GetBacteriaForNeighbours(Dictionary<uint, int> sourceGeneNeighbours)
		{
			return sourceGeneNeighbours.Keys.ToDictionary(gene => gene, gene => CleanedData[gene]);
		}

		private Dictionary<uint, float> CalculateProbabilitiesWithGenes(uint gene, Dictionary<uint, string[]> bacteriasForGene)
		{
			var probabilities = new Dictionary<uint, float>(bacteriasForGene.Count);

			foreach (var j in bacteriasForGene.Keys)
			{
				if (gene != j)
				{
					var probability = CalculateGeneProbability(bacteriasForGene[gene], bacteriasForGene[j]);

					if (probability >= ProbabilityThreshold)
					{
						probabilities[j] = probability;
					}
				}
			}

			return probabilities;
		}

		private void FilterByThresholdProbability(Dictionary<uint, string[]> bacteriaForNeighbours, Dictionary<uint, int> sourceGeneNeighbours)
		{
			var toDelete = bacteriaForNeighbours.Keys.Where(gene => !GeneNeighboursProbabilities.ContainsKey(gene)).ToList();

			toDelete.ForEach(gene => sourceGeneNeighbours.Remove(gene));
		}


		protected override Dictionary<uint, Dictionary<uint, float>> GetProbabilitiesNetwork(Dictionary<uint, string[]> bacteriaForNeighbours)
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
	}
}