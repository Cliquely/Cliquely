namespace CliquesForGenome
{
    public class Genome
	{
        public string Taxonomy { get; set; }
        public string Abbr { get; set; }
        public string Name { get; set; }


        public Genome(string taxonomy, string abbr, string name)
        {
            this.Taxonomy = taxonomy;
            this.Abbr = abbr;
            this.Name = name;
        }


        public override string ToString()
        {
            return $"{Taxonomy} {Abbr} {Name}";
        }
    }
}