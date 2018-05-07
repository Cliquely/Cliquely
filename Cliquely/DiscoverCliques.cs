using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliquely
{
    public class DiscoverCliques
    {
        uint gene;
        Dictionary<uint, Dictionary<uint, float>> probabilities;

        public List<List<uint>> Cliques;

        public DiscoverCliques(uint i_Gene, Dictionary<uint, Dictionary<uint, float>> i_Probabilities)
        {
            gene = i_Gene;
            probabilities = i_Probabilities;
            Cliques = new List<List<uint>>();
        }

        public void Run()
        {
            List<uint> excludedVertices = new List<uint>();
            List<uint> possibleCliqueVertices = new List<uint>(probabilities.Keys);

            BronKerbosch2(
                new List<uint> { gene },
                possibleCliqueVertices,
                excludedVertices
            );

        }

        private void BronKerbosch2(List<uint> i_CliqueVertices, List<uint> i_PossibleCliqueVertices,
            List<uint> i_ExcludedVertices) // R P X
        {
            /* if (i_CliqueVertices.Count > 50)
             {
                 return;
             }*/

            if (i_PossibleCliqueVertices.Count == 0 && i_ExcludedVertices.Count == 0)
            {
                if (i_CliqueVertices.Count > 1)
                {
                    notifyNewClique(i_CliqueVertices);
                }

                return;
            }

            uint pivot = selectMaximumDegreeVertex(i_PossibleCliqueVertices.Union(i_ExcludedVertices));

            List<uint> enumerableVertices = i_PossibleCliqueVertices.Except(getNeighbours(pivot)).ToList();

            for (int i = 0; i < enumerableVertices.Count;)
            {
                IEnumerable<uint> vertexNeighbours = getNeighbours(enumerableVertices[i]);

                BronKerbosch2(
                    i_CliqueVertices.Union(new List<uint> { enumerableVertices[i] }).ToList(),
                    i_PossibleCliqueVertices.Intersect(vertexNeighbours).ToList(),
                    i_ExcludedVertices.Intersect(vertexNeighbours).ToList()
                );

                i_ExcludedVertices.Add(enumerableVertices[i]);
                i_PossibleCliqueVertices.Remove(enumerableVertices[i]);

                enumerableVertices = i_PossibleCliqueVertices.Except(getNeighbours(pivot)).ToList();
            }
        }

        private void notifyNewClique(List<uint> i_CliqueVertices)
        {
            Cliques.Add(i_CliqueVertices);
        }

        private IEnumerable<uint> getNeighbours(uint i_Id)
        {
            return probabilities[i_Id].Keys;
        }

        private uint selectMaximumDegreeVertex(IEnumerable<uint> i_Union)
        {
            int maxDegree = probabilities.Where(e => i_Union.Contains(e.Key)).Max(e => e.Value.Count);

            return probabilities.First(e => i_Union.Contains(e.Key) & e.Value.Count == maxDegree).Key;
        }
    }
}