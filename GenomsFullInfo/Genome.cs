namespace GenomsFullInfo
{
    public class Genome
    {
        public string Abbreviation { get; set; }
        public string FullName { get; set; }
        public string Taxonomy { get; set; }
        public int NumberOfGenes { get; set; }
        public int NumberOfClusters { get; set; }
        public int NumberOfCliques { get; set; }

        public override int GetHashCode()
        {
            return Abbreviation.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Genome genome)
            {
                return genome.Abbreviation == Abbreviation;
            }

            return false;
        }
    }
}
