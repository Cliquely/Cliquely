using System.Collections.Generic;

namespace BacteriaNetworks
{
    public class Gene
    {
        public uint Id { get; }
        public List<Gene> Neighbors { get; }

        public Gene(uint id, List<Gene> neighbors)
        {
            Id = id;
            Neighbors = neighbors;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((Gene)obj).Id == Id;
        }
    }
}