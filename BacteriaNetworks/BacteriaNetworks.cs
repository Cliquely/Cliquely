using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BacteriaNetworks
{
	public partial class BacteriaNetworks
    {
        public List<List<Gene>> SearchNetworks(string bacteria, float minimumProbability)
        {
            var genesOfBacteria = GetGenesOfBacteria(bacteria, minimumProbability);

            return new BronKerboschAlg().Run(genesOfBacteria);
        }

        private List<Gene> GetGenesOfBacteria(string bacteria, float minimumProbability)
        {
            var cleanedDataReader = new CleanedDataReader();
	        var bacteriaForProteins = cleanedDataReader.ReadAllBacteriaForProteins();

			var cleanedDataReverseReader = new CleanedDataReverseReader();
	        var proteinsForBacteria = cleanedDataReverseReader.ReadProteinsForBacteriaByName(bacteria);

	        var bacterialNetworkCalculator = new BacterialNetworkCalculator(bacteriaForProteins)
	        {
		        ThresholdProbability = minimumProbability
	        };

	        var bacterialNetwork = bacterialNetworkCalculator.CalculateForOneBacteria(proteinsForBacteria);

	        return ConvertBacterialNetworkToGeneList(bacterialNetwork);
        }

	    private List<Gene> ConvertBacterialNetworkToGeneList(Dictionary<uint, Dictionary<uint, float>> bacterialNetwork)
	    {
		    return new BacterialNetworkToGeneConverter(bacterialNetwork).Convert();
	    }
	}
}
