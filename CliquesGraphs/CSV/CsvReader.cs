using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CliquesGraphs.CSV
{
    public class CsvReader : IDisposable
    {
        private const int TAXONOMY_LOCATION = 0;
        private const int ABBREV_LOCATION = 2;
        private const int SIZE_LOCATION = 3;
        private const int GENES_LOCATION = 4;
        
        private readonly TextReader reader;

        public CsvReader(TextReader reader, bool hasHeader = true)
        {
            this.reader = reader;
            reader.ReadLine();
        }

        public List<CliqueRecord> ReadCSV()
        {
            var records = new List<CliqueRecord>();
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                var cliqueRecord = line.Split(',');

                var record = new CliqueRecord() { Taxonomy = Enum.Parse<eTaxonomy>(cliqueRecord[TAXONOMY_LOCATION]), Abbrev = cliqueRecord[ABBREV_LOCATION], Size = ushort.Parse(cliqueRecord[SIZE_LOCATION]), Genes = new List<uint>() };
                for (var i = GENES_LOCATION; i < cliqueRecord.Length;i++)
                {
                    record.Genes.Add(uint.Parse(cliqueRecord[i]));
                }

                record.Genes.Sort();

                if (!records.Exists(r => r.Genes.SequenceEqual(record.Genes) && r.Taxonomy == record.Taxonomy))
                {
                    records.Add(record);
                }
            }

            return records;
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
