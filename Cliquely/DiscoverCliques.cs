using System;
using System.Collections.Generic;
using System.Linq;

namespace Cliquely
{
    public class DiscoverCliques
    {
	    private uint Gene { get; }
	    private Random Rand { get; } = new Random();
	    private IEnumerable<uint> PotentialGenes { get; }
		private int MaxCliqueSize { get; }
	    private Dictionary<uint, Dictionary<uint, float>> GenesNetwork { get; set; }

		private int MaxCliques { get; }

        public readonly List<List<uint>> Cliques;

        public DiscoverCliques(uint gene, IEnumerable<uint> potentialGenes, Dictionary<uint, Dictionary<uint, float>> genesNetwork, int maxCliqueSize, int maxCliques)
        {
	        Gene = gene;
	        PotentialGenes = potentialGenes;
            Cliques = new List<List<uint>>();
	        MaxCliqueSize = maxCliqueSize;
            MaxCliques = maxCliques;
	        GenesNetwork = genesNetwork;
        }

        public void Run()
        {
            var excludedVertices = new List<uint>();
            var possibleCliqueVertices = new List<uint>(PotentialGenes);

	        BronKerbosch2(
                new List<uint> { Gene },
                possibleCliqueVertices,
                excludedVertices
            );

        }

        private void BronKerbosch2(List<uint> cliqueVertices, List<uint> possibleCliqueVertices, List<uint> excludedVertices) // R P X
        { 
			if (possibleCliqueVertices.Count == 0 && excludedVertices.Count == 0)
            {
                if (cliqueVertices.Count > 1)
                {
                    NotifyNewClique(cliqueVertices);
                }

                return;
            }

            if (cliqueVertices.Count == MaxCliqueSize)
            {
                return;
            }

            if(Cliques.Count >= MaxCliques)
            {
                return;
            }

            var pivot = SelectRandomVertex(possibleCliqueVertices.Union(excludedVertices));
            var enumerableVertices = possibleCliqueVertices.Except(getNeighbours(pivot)).ToList();

            for (var i = 0; i < enumerableVertices.Count;)
            {
                var vertexNeighbours = getNeighbours(enumerableVertices[i]).ToList();

                BronKerbosch2(
                    cliqueVertices.Union(new List<uint> { enumerableVertices[i] }).ToList(),
                    possibleCliqueVertices.Intersect(vertexNeighbours).ToList(),
                    excludedVertices.Intersect(vertexNeighbours).ToList()
                );

                excludedVertices.Add(enumerableVertices[i]);
                possibleCliqueVertices.Remove(enumerableVertices[i]);

                enumerableVertices = possibleCliqueVertices.Except(getNeighbours(pivot)).ToList();
            }
        }

        private void NotifyNewClique(List<uint> cliqueVertices)
        {
            Cliques.Add(cliqueVertices);
        }

        private IEnumerable<uint> getNeighbours(uint id)
        {
            return GenesNetwork[id].Keys;
        }

        private uint SelectMaximumDegreeVertex(IEnumerable<uint> union)
        {
			var maxDegree = GenesNetwork.Where(e => union.Contains(e.Key)).Max(e => e.Value.Count);

            return GenesNetwork.First(e => union.Contains(e.Key) & e.Value.Count == maxDegree).Key;
		}

		private uint SelectRandomVertex(IEnumerable<uint> union)
		{
			var unionList = union.ToList();

			return unionList[Rand.Next(0, unionList.Count)];
		}
	}
}