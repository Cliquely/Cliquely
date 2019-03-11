using System.Collections.Generic;
using System.Linq;

namespace CliquesForGenome
{
    public class BronKerboschAlg
    {
        private readonly int maximumCliqueSize;

        public BronKerboschAlg(int maximumCliqueSize)
        {
            this.maximumCliqueSize = maximumCliqueSize;
        }

        public List<List<Gene>> Run(List<Gene> genes)
        {
            return BronKerbosch2(
                new List<Gene>(),
                new List<Gene>(genes),
                new List<Gene>()
            );
        }

        private List<List<Gene>> BronKerbosch2(List<Gene> cliqueVertices, List<Gene> possibleCliqueVertices,
            List<Gene> excludedVertices) // R P X
        {
            var cliques = new List<List<Gene>>();

            if ((possibleCliqueVertices.Count == 0 && excludedVertices.Count == 0) || (maximumCliqueSize != -1 && cliqueVertices.Count >= maximumCliqueSize))
            {
                if (cliqueVertices.Count > 1)
                {
                    cliques.Add(cliqueVertices);
                }

                return cliques;
            }

            var pivot = selectMaximumDegreeVertex(possibleCliqueVertices.Union(excludedVertices));

            var enumerableVertices = possibleCliqueVertices.Except(pivot.Neighbors).ToList();

            foreach (var vertex in enumerableVertices)
            {
                cliques.AddRange(BronKerbosch2(
                    cliqueVertices.Union(new List<Gene> { vertex }).ToList(),
                    possibleCliqueVertices.Intersect(vertex.Neighbors).ToList(),
                    excludedVertices.Intersect(vertex.Neighbors).ToList()
                ));

                possibleCliqueVertices.Remove(vertex);
                excludedVertices.Add(vertex);
            }

            return cliques;
        }

        private Gene selectMaximumDegreeVertex(IEnumerable<Gene> genes)
        {
            return genes.Aggregate((g1, g2) => g1.Neighbors.Count > g2.Neighbors.Count ? g1 : g2);
        }
    }
}