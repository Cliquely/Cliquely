using System.Collections.Generic;
using System.Linq;

namespace CliquesForGenome.Network.Calculators
{
	public class RunTimeGenomeNetworkCalculator : GenomeNetworkCalculator
	{
		public Dictionary<uint, List<string>> GenomesForGenes { get; set; }

		public RunTimeGenomeNetworkCalculator(Dictionary<uint, List<string>> genomesForGenes)
		{
			GenomesForGenes = genomesForGenes;
		}

		public Dictionary<uint, Dictionary<uint, float>> CalculateForOneGenome(List<uint> genesInGenome)
		{
			return Calculate(genesInGenome);
		}

		public Dictionary<uint, Dictionary<uint, float>> CalculateAll()
		{
			return Calculate(GenomesForGenes.Keys.ToList());
		}

		protected override float CalcProbability(uint firstGeneId, uint secondGeneId)
		{
			var firstGeneGenome = GenomesForGenes[firstGeneId];
			var secondGeneGenome = GenomesForGenes[secondGeneId];

			return ProbabilitiesCalculator.CalculateGeneProbability(firstGeneGenome, secondGeneGenome);
		}
	}
}
