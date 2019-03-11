using System.Collections.Generic;
using System.Linq;

namespace CliquesForGenome.Network
{
	public static class ProbabilitiesCalculator
    {
        public static float CalculateGeneProbability(IReadOnlyCollection<string> i_GenomesForGene1, IReadOnlyCollection<string> i_GenomesForGene2)
        {
            var amountOfIntersectedGenomes = i_GenomesForGene1.Intersect(i_GenomesForGene2).Count();

            float numerator = amountOfIntersectedGenomes * amountOfIntersectedGenomes;
            float denominator = i_GenomesForGene1.Count * i_GenomesForGene2.Count;

            return numerator / denominator;
        }
    }
}