using CliquesForGenome.Configuration;
using System.Collections.Generic;
using System.IO;

namespace CliquesForGenome.Network.Readers
{
    public class GenomeLineReader
    {
        public IEnumerable<Genome> ReadAllGenomes()
        {
            using (var genomesFile = new StreamReader(Settings.Instance.GenomesFile))
            {
                while (!genomesFile.EndOfStream)
                {
                    var line = genomesFile.ReadLine();

                    yield return parseGenome(line);
                }
            }
        }

        private Genome parseGenome(string line)
        {
            var splitedLine = line.Split('\t');

            return new Genome(splitedLine[0], splitedLine[1], splitedLine[2]);
        }
    }
}