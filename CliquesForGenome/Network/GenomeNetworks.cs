using CliquesForGenome.Network.Calculators;
using CliquesForGenome.Network.Readers;
using System.Collections.Generic;

namespace CliquesForGenome.Network
{
	public partial class GenomeNetworks
    {
        private Dictionary<uint, List<string>> genomeForGenes;
        private RunTimeGenomeNetworkCalculator genomelNetworkCalculator;
        private BronKerboschAlg bronKerbosch;
        private CleanedDataReverseReader cleanedDataReverseReader;

        public GenomeNetworks(float minimumProbability, int maximumCliqueSize)
        {
            genomeForGenes = new CleanedDataReader().ReadAllGenomeForGenes();
            cleanedDataReverseReader = new CleanedDataReverseReader();
            genomelNetworkCalculator = new RunTimeGenomeNetworkCalculator(genomeForGenes) { ThresholdProbability = minimumProbability };
            bronKerbosch = new BronKerboschAlg(maximumCliqueSize);
        }

        public List<List<Gene>> SearchNetworks(string genome)
        {
            var genesOfGenome = GetGenesOfGenome(genome);

            return bronKerbosch.Run(genesOfGenome);
        }

        private List<Gene> GetGenesOfGenome(string genome)
        {
	        var genesForGenome = cleanedDataReverseReader.ReadGenesForGenomeByName(genome);
	        var genomelNetwork = genomelNetworkCalculator.CalculateForOneGenome(genesForGenome);

	        return ConvertGenomelNetworkToGeneList(genomelNetwork);
		}

	    private List<Gene> ConvertGenomelNetworkToGeneList(Dictionary<uint, Dictionary<uint, float>> genomelNetwork)
	    {
		    return new GenomeNetworkToGeneConverter(genomelNetwork).Convert();
	    }
	}
}
