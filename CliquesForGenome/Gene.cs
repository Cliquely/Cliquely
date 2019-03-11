using System.Collections.Generic;

namespace CliquesForGenome
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
            return obj != null && ((Gene)obj).Id == Id;
        }
    }
}