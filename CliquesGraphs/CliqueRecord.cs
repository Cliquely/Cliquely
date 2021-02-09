using System;
using System.Collections.Generic;

namespace CliquesGraphs
{
    public class CliqueRecord
    {
        public eTaxonomy Taxonomy { get; set; }

        public string Abbrev { get; set; }

        public ushort Size { get; set; }

        public List<uint> Genes { get; set; }

        public override bool Equals(object obj)
        {
            return GetHashCode().Equals((obj as CliqueRecord).GetHashCode());
        }

        public override int GetHashCode()
        {
            return new { Taxonomy, Abbrev, Size, Genes }.GetHashCode();
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
