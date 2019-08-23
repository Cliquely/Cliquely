using System;
using System.Collections.Generic;
using System.Linq;

namespace Cliquely
{
    public class DiscoverCliques
    {
	    protected Random Rand { get; } = new Random();
	    protected IEnumerable<uint> PotentialGenes { get; }
		protected int MaxCliqueSize { get; }
	    protected Dictionary<uint, Dictionary<uint, float>> GenesNetwork { get; }

		protected int MaxCliques { get; }

        public readonly List<List<uint>> Cliques;

        public DiscoverCliques(IEnumerable<uint> potentialGenes, Dictionary<uint, Dictionary<uint, float>> genesNetwork, int maxCliqueSize, int maxCliques)
        {
	        PotentialGenes = potentialGenes;
            Cliques = new List<List<uint>>();
	        MaxCliqueSize = maxCliqueSize;
            MaxCliques = maxCliques;
	        GenesNetwork = genesNetwork;
        }

        public virtual void Run()
        {
	        BronKerbosch2(
		        cliqueVertices: new List<uint> (),
		        possibleCliqueVertices: new List<uint>(PotentialGenes),
		        excludedVertices: new List<uint>()
			);

        }

        protected virtual void BronKerbosch2(List<uint> cliqueVertices, List<uint> possibleCliqueVertices, List<uint> excludedVertices) // R P X
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

            for (var i = 0; i < possibleCliqueVertices.Count;)
            {
                var vertexNeighbours = getNeighbours(possibleCliqueVertices[i]).ToList();

                BronKerbosch2(
                    cliqueVertices.Union(new List<uint> { possibleCliqueVertices[i] }).ToList(),
                    possibleCliqueVertices.Intersect(vertexNeighbours).ToList(),
                    excludedVertices.Intersect(vertexNeighbours).ToList()
                );

                excludedVertices.Add(possibleCliqueVertices[i]);
                possibleCliqueVertices.Remove(possibleCliqueVertices[i]);
            }
        }

        protected virtual void NotifyNewClique(List<uint> cliqueVertices)
        {
            Cliques.Add(cliqueVertices);
        }

        protected IEnumerable<uint> getNeighbours(uint id)
        {
            return GenesNetwork[id].Keys;
        }

        protected uint SelectMaximumDegreeVertex(IEnumerable<uint> union)
        {
			var maxDegree = GenesNetwork.Where(e => union.Contains(e.Key)).Max(e => e.Value.Count);

            return GenesNetwork.First(e => union.Contains(e.Key) & e.Value.Count == maxDegree).Key;
		}

		protected uint SelectRandomVertex(IEnumerable<uint> union)
		{
			var unionList = union.ToList();

			return unionList[Rand.Next(0, unionList.Count)];
		}
	}
}