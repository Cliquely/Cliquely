using System.Collections.Generic;

namespace Cliquely
{
	public class DiscoverCliquesByGene : DiscoverCliques
	{
		private uint Gene { get; }

		public DiscoverCliquesByGene(uint gene, IEnumerable<uint> potentialGenes, Dictionary<uint, Dictionary<uint, float>> genesNetwork, int maxCliqueSize, int maxCliques)
			: base(potentialGenes, genesNetwork, maxCliqueSize, maxCliques)
		{
			Gene = gene;
		}

		public override void Run()
		{
			var excludedVertices = new List<uint>();
			var possibleCliqueVertices = new List<uint>(PotentialGenes);

			BronKerbosch2(
				new List<uint> { Gene },
				possibleCliqueVertices,
				excludedVertices
			);

		}
	}
}