using System.Collections.Generic;
using System.Linq;

namespace BacteriaNetworks
{
	public partial class BacteriaNetworks
    {
		class BacterialNetworkToGeneConverter
	    {
		    private Dictionary<uint, Dictionary<uint, float>> BacterialNetwork { get; set; }
			private Dictionary<uint, Gene> Genes { get; } = new Dictionary<uint, Gene>();

		    public BacterialNetworkToGeneConverter(Dictionary<uint, Dictionary<uint, float>> bacterialNetwork)
		    {
			    BacterialNetwork = bacterialNetwork;
		    }

		    public List<Gene> Convert()
		    {
			    return BacterialNetwork.Select(x => GetGene(x.Key)).ToList();
		    }

		    private Gene GetGene(uint proteinId)
		    {
			    if (!Genes.ContainsKey(proteinId))
			    {
					var newGene = new Gene(proteinId, new List<Gene>());
				    Genes[proteinId] = newGene;

					// Add neighbors after ctor to prevent stack overflow by recursion 
				    Genes[proteinId].Neighbors.AddRange(BacterialNetwork[proteinId].Keys.Select(GetGene).ToList());

			    }

				return Genes[proteinId];
		    }
	    }
	}
}
