using System;

namespace CliquesGraphs
{
    public class CliqueRecord
    {
        public eTaxonomy Taxonomy { get; set; }

        public string Abbrev { get; set; }

        public int Size { get; set; }

        public string Genes { get; set; }

        public override bool Equals(object obj)
        {
            return Genes.Equals((obj as CliqueRecord).Genes);
        }

        public override int GetHashCode()
        {
            return Genes.GetHashCode();
        }
    }

    [Flags]
    public enum eTaxonomy
    {
        Archaea = 1,
        Bacteria = 2,
        Eukaryota = 4
    }
}
