using System;
using Cliquely;

namespace CliquesGraphs
{
    public class CliqueRecord
    {
        public eTaxonomy Taxonomy { get; set; }

        public string Abbrev { get; set; }

        public ushort Size { get; set; }

        public override bool Equals(object obj)
        {
            return GetHashCode().Equals((obj as CliqueRecord).GetHashCode());
        }

        public override int GetHashCode()
        {
            return Taxonomy.GetHashCode() + Abbrev.GetHashCode() + Size.GetHashCode();
        }
    }
}
