using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cliquely;

namespace BacteriaNetworks
{
	public class BacterialNetworkCalculator
    {
	    public float ThresholdProbability { get; set; } = 0.7f;
	    public Dictionary<uint, List<string>> BacteriasForProteins { get; set; }
	    public Dictionary<uint, Dictionary<uint, float>> BacterialNetwork { get; set; }

	    public BacterialNetworkCalculator(Dictionary<uint, List<string>> bacteriasForProteins)
	    {
		    BacteriasForProteins = bacteriasForProteins;
	    }

	    public Dictionary<uint, Dictionary<uint, float>> CalculateForOneBacteria(List<uint> proteinForBacteria)
	    {
		    BacterialNetwork = new Dictionary<uint, Dictionary<uint, float>>();

			foreach (var firstProteinId in proteinForBacteria)
			{
				foreach (var secondProteinId in proteinForBacteria)
				{
					if (firstProteinId != secondProteinId)
					{
						var firstProteinBacteria = BacteriasForProteins[firstProteinId];
						var secondProteinBacteria = BacteriasForProteins[secondProteinId];
						var proteinsProbability =
							ProbabilitiesCalculator.CalculateGeneProbability(firstProteinBacteria,
								secondProteinBacteria);

						AddProteinsProbabilityToNetwork(proteinsProbability, firstProteinId, secondProteinId);
					}
				}
			}

		    return BacterialNetwork;
	    }

		public Dictionary<uint, Dictionary<uint, float>> Calculate()
	    {
		    var bacterialNetwork = new Dictionary<uint, Dictionary<uint, float>>();
		    var bacteriasForProteinsKeys = BacteriasForProteins.Keys.ToList();

			foreach (var firstProteinId in bacteriasForProteinsKeys)
			{
				foreach (var secondProteinId in bacteriasForProteinsKeys)
				{
					var firstProteinBacteria = BacteriasForProteins[firstProteinId];
					var secondProteinBacteria = BacteriasForProteins[secondProteinId];
					var proteinsProbability = ProbabilitiesCalculator.CalculateGeneProbability(firstProteinBacteria, secondProteinBacteria);

					AddProteinsProbabilityToNetwork(proteinsProbability, firstProteinId, secondProteinId);
				}
			}

			return bacterialNetwork;
	    }

		public void CalculateAndExport()
	    {
		    const string initQuery = "INSERT INTO `HomologyProteinsProbabilities`(`firstProteinId`,`secondProteinId`,`probability`) VALUES ";

		    var bacteriasForProteinsKeys = BacteriasForProteins.Keys.ToList();
			var queryBuilder = new StringBuilder(initQuery);
		    var k = 0;

			for (var i = 1; i < bacteriasForProteinsKeys.Count; i++)
			{
				for (var j = 0; j < i; j++)
				{
					var firstProteinId = bacteriasForProteinsKeys[i];
					var secondProteinId = bacteriasForProteinsKeys[j];
					var firstProteinBacteria = BacteriasForProteins[firstProteinId];
					var secondProteinBacteria = BacteriasForProteins[secondProteinId];
					var proteinsProbability = ProbabilitiesCalculator.CalculateGeneProbability(firstProteinBacteria, secondProteinBacteria);

					if (proteinsProbability >= ThresholdProbability)
				    {
					    queryBuilder.Append($"({firstProteinId},{secondProteinId},{proteinsProbability}),");
					    k++;

					    if (k == 1000)
					    {
						    var query = queryBuilder.Replace(',', ';', queryBuilder.Length - 1, 1).ToString();
						    new SqlHelper("BacterialNetwork.data").Edit(query);

						    queryBuilder.Clear();
						    queryBuilder.Append(initQuery);
						    k = 0;
					    }
				    }
			    }
		    }
	    }

	    private void AddProteinsProbabilityToNetwork(float proteinsProbability, uint firstProteinId, uint secondProteinId)
	    {
		    if (proteinsProbability < ThresholdProbability) return;

		    if (!BacterialNetwork.ContainsKey(firstProteinId))
		    {
			    BacterialNetwork.Add(firstProteinId, new Dictionary<uint, float>());
		    }

		    BacterialNetwork[firstProteinId].Add(secondProteinId, proteinsProbability);
	    }
    }
}
