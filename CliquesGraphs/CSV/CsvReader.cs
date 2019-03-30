using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CliquesGraphs.CSV
{
    public class CsvReader : IDisposable
    {
        private readonly TextReader reader;

        public CsvReader(TextReader reader, bool hasHeader = true)
        {
            this.reader = reader;
            reader.ReadLine();
        }

        public IEnumerable<CliqueRecord> ReadCliques()
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                var cliqueRecord = reader.ReadLine().Split(',');

                yield return new CliqueRecord() { Size = int.Parse(cliqueRecord[3]), Genes = string.Join(',', cliqueRecord.Skip(4))};
            }
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
