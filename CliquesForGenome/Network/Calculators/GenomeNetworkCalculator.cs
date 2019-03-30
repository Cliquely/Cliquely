using CliquesForGenome.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CliquesForGenome.Network.Calculators
{
	public abstract class GenomeNetworkCalculator
	{
		public float ThresholdProbability { get; set; } = 0.7f;

        private Dictionary<uint, Dictionary<uint, float>> genesProbabilities = new Dictionary<uint, Dictionary<uint, float>>();

		protected virtual Dictionary<uint, Dictionary<uint, float>> Calculate(List<uint> genes)
		{
			var genomelNetwork = new Dictionary<uint, Dictionary<uint, float>>();

			foreach (var firstGeneId in genes)
			{
				foreach (var secondGeneId in genes.TakeWhile(gene => gene < firstGeneId))
				{
					var genesProbability = GetProbability(firstGeneId, secondGeneId);

					AddGenesProbabilityToNetwork(genomelNetwork, genesProbability, firstGeneId, secondGeneId);
				}
			}

			return genomelNetwork;
		}

        private float GetProbability(uint firstGeneId, uint secondGeneId)
        {
            if (genesProbabilities.ContainsKey(firstGeneId) && genesProbabilities[firstGeneId].ContainsKey(secondGeneId))
            {
                return genesProbabilities[firstGeneId][secondGeneId];
            }

            if (genesProbabilities.ContainsKey(secondGeneId) && genesProbabilities[secondGeneId].ContainsKey(firstGeneId))
            {
                return genesProbabilities[secondGeneId][firstGeneId];
            }

            var probability = CalcProbability(firstGeneId, secondGeneId);

            if (probability >= Settings.Instance.MinimumProbabilityCache)
            {
                if (!genesProbabilities.ContainsKey(firstGeneId))
                    genesProbabilities.Add(firstGeneId, new Dictionary<uint, float>());

                genesProbabilities[firstGeneId].Add(secondGeneId, probability);
            }

            return probability;
        }

		protected abstract float CalcProbability(uint firstGeneId, uint secondGeneId);

		private void AddGenesProbabilityToNetwork(Dictionary<uint, Dictionary<uint, float>> genomelNetwork, float genesProbability, uint firstGeneId, uint secondGeneId)
		{
			if (genesProbability < ThresholdProbability) return;

            if (!genomelNetwork.ContainsKey(firstGeneId))
            {
                genomelNetwork.Add(firstGeneId, new Dictionary<uint, float>());
            }

            if (!genomelNetwork.ContainsKey(secondGeneId))
            {
                genomelNetwork.Add(secondGeneId, new Dictionary<uint, float>());
            }

            genomelNetwork[firstGeneId].Add(secondGeneId, genesProbability);
            genomelNetwork[secondGeneId].Add(firstGeneId, genesProbability);
        }
	}
}