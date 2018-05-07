using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlastFromNCBI
{
    public struct BlastGene
    {
        public string Sequence;
        public float MatchingPercentage;

        public BlastGene(string i_Sequence, float i_MatchingPercentage)
        {
            Sequence = i_Sequence;
            MatchingPercentage = i_MatchingPercentage;
        }

        public override string ToString()
        {
            return string.Format($"{MatchingPercentage} : {Sequence}");
        }
    }
}