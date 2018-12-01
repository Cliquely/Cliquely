using System.Collections.Generic;
using System.Linq;
using Cliquely;

namespace BacteriaNetworks.Infrastructure.Network.Calculators
{
	public class RunTimeBacterialNetworkCalculator : BacterialNetworkCalculator
	{
		public Dictionary<uint, List<string>> BacteriasForProteins { get; set; }

		public RunTimeBacterialNetworkCalculator(Dictionary<uint, List<string>> bacteriasForProteins)
		{
			BacteriasForProteins = bacteriasForProteins;
		}

		public Dictionary<uint, Dictionary<uint, float>> CalculateForOneBacteria(List<uint> proteinsInBacteria)
		{
			return Calculate(proteinsInBacteria);
		}

		public Dictionary<uint, Dictionary<uint, float>> CalculateAll()
		{
			return Calculate(BacteriasForProteins.Keys.ToList());
		}

		protected override float GetProbability(uint firstProteinId, uint secondProteinId)
		{
			var firstProteinBacteria = BacteriasForProteins[firstProteinId];
			var secondProteinBacteria = BacteriasForProteins[secondProteinId];

			return ProbabilitiesCalculator.CalculateGeneProbability(firstProteinBacteria, secondProteinBacteria);
		}
	}
}
