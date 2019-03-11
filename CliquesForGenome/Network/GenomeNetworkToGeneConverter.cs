using System.Collections.Generic;
using System.Linq;

namespace CliquesForGenome.Network
{
	public partial class GenomeNetworks
    {
		class GenomeNetworkToGeneConverter
	    {
		    private Dictionary<uint, Dictionary<uint, float>> GenomelNetwork { get; set; }
			private Dictionary<uint, Gene> Genes { get; } = new Dictionary<uint, Gene>();

		    public GenomeNetworkToGeneConverter(Dictionary<uint, Dictionary<uint, float>> genomelNetwork)
		    {
			    GenomelNetwork = genomelNetwork;
		    }

		    public List<Gene> Convert()
		    {
			    return GenomelNetwork.Select(x => GetGene(x.Key)).ToList();
		    }

		    private Gene GetGene(uint geneId)
		    {
			    if (!Genes.ContainsKey(geneId))
			    {
					var newGene = new Gene(geneId, new List<Gene>());
				    Genes[geneId] = newGene;

					// Add neighbors after ctor to prevent stack overflow by recursion 
				    Genes[geneId].Neighbors.AddRange(GenomelNetwork[geneId].Keys.Select(GetGene).ToList());

			    }

				return Genes[geneId];
		    }
	    }
	}
}
