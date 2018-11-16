using System.Linq;

namespace BacteriaNetworks
{
    public class Program
    {
        public static void Main()
        {
			//var bacteriaNetworks = new BacteriaNetworks();
			//bacteriaNetworks.SearchNetworks("ztr", 0.7f);

			var cleanedDataReader = new CleanedDataReader();
	        var bacteriaForProteins = cleanedDataReader.ReadAllBacteriaForProteins();
	        var proteins = bacteriaForProteins.Keys.ToList();

	        var bacterialNetworkDbWriter = new BacterialNetworkDbWriter();
	        bacterialNetworkDbWriter.WriteProteins(proteins);

	        var bacterialNetworkCalculator = new BacterialNetworkCalculator(bacteriaForProteins);
	        bacterialNetworkCalculator.CalculateAndExport();
        }
	}
}
