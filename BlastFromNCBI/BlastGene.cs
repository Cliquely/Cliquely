namespace BlastFromNCBI
{
	public struct BlastGene
    {
        public string Sequence;
        public float MatchingPercentage;
		public string AccessionId { get; set; }

		public BlastGene(string i_Sequence, float i_MatchingPercentage, string i_AccessionId)
        {
            Sequence = i_Sequence;
            MatchingPercentage = i_MatchingPercentage;
			AccessionId = i_AccessionId;
        }

        public override string ToString()
        {
            return string.Format($"{MatchingPercentage} : {Sequence}");
        }
    }
}