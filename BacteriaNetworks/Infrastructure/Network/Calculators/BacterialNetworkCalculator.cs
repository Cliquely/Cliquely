using System.Collections.Generic;
using System.Text;
using Cliquely;

namespace BacteriaNetworks.Infrastructure.Network.Calculators
{
	public abstract class BacterialNetworkCalculator
	{
		public float ThresholdProbability { get; set; } = 0.7f;

		protected virtual Dictionary<uint, Dictionary<uint, float>> Calculate(List<uint> proteins)
		{
			var bacterialNetwork = new Dictionary<uint, Dictionary<uint, float>>();

			foreach (var firstProteinId in proteins)
			{
				foreach (var secondProteinId in proteins)
				{
					var proteinsProbability = GetProbability(firstProteinId, secondProteinId);

					AddProteinsProbabilityToNetwork(bacterialNetwork, proteinsProbability, firstProteinId, secondProteinId);
				}
			}

			return bacterialNetwork;
		}

		protected abstract float GetProbability(uint firstProteinId, uint secondProteinId);

		public void CalculateAndDbExport(List<uint> proteins)
		{
			const string initQuery = "INSERT INTO `HomologyProteinsProbabilities`(`firstProteinId`,`secondProteinId`,`probability`) VALUES ";

			var queryBuilder = new StringBuilder(initQuery);
			var k = 0;

			for (var i = 1; i < proteins.Count; i++)
			{
				for (var j = 0; j < i; j++)
				{
					var firstProteinId = proteins[i];
					var secondProteinId = proteins[j];

					var proteinsProbability = GetProbability(firstProteinId, secondProteinId);

					if (proteinsProbability >= ThresholdProbability)
					{
						queryBuilder.Append($"({firstProteinId},{secondProteinId},{proteinsProbability}),");
						k++;

						if (k == 1000)
						{
							var query = queryBuilder.Replace(',', ';', queryBuilder.Length - 1, 1).ToString();
							new SqlHelper(BacterialNetworkDbWriter.DefaultDB).Edit(query);

							queryBuilder.Clear();
							queryBuilder.Append(initQuery);
							k = 0;
						}
					}
				}
			}

			if (k > 0)
			{
				var query = queryBuilder.Replace(',', ';', queryBuilder.Length - 1, 1).ToString();
				new SqlHelper(BacterialNetworkDbWriter.DefaultDB).Edit(query);
			}
		}

		protected void AddProteinsProbabilityToNetwork(Dictionary<uint, Dictionary<uint, float>> bacterialNetwork, float proteinsProbability, uint firstProteinId, uint secondProteinId)
		{
			if (proteinsProbability < ThresholdProbability) return;

			if (!bacterialNetwork.ContainsKey(firstProteinId))
			{
				bacterialNetwork.Add(firstProteinId, new Dictionary<uint, float>());
			}

			bacterialNetwork[firstProteinId].Add(secondProteinId, proteinsProbability);
		}
	}
}