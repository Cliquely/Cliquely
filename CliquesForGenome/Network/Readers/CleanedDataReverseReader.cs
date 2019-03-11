using CliquesForGenome.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CliquesForGenome.Network.Readers
{
    public class CleanedDataReverseReader
    {
        public List<uint> ReadGenesForGenomeByName(string name)
        {
            try
            {
                using (var cleanedDataReverseFileReader = new FileStream(Settings.Instance.GenesForGenomesFile, FileMode.Open))
                using (var cleanedDataReverseStreamReader = new StreamReader(cleanedDataReverseFileReader))
                {
                    return ParseOne(cleanedDataReverseStreamReader, name);
                }
            }
            catch (Exception e)
            {
                throw new Exception("The format of cleaned data reverse file is wrong.", e);
            }
        }

        private List<uint> ParseOne(StreamReader reader, string name)
        {
            SkipInfoLines(reader);

            var line = reader.ReadLine();

            while (line != null)
            {
                var lineName = getGeneLineName(line);

                if (lineName == name)
                {
                    return ParseGenesForGenomesPair(line).Value;
                }

                line = reader.ReadLine();
            }

            return new List<uint>();
        }

        private string getGeneLineName(string line)
        {
            return line.Split('\t')[0];
        }

        public List<string> ReadAllGenome()
        {
            try
            {
                using (var cleanedDataReverseFileReader = new FileStream(Settings.Instance.GenesForGenomesFile, FileMode.Open))
                using (var cleanedDataReverseStreamReader = new StreamReader(cleanedDataReverseFileReader))
                {
                    return ParseAllGenome(cleanedDataReverseStreamReader);
                }
            }
            catch (Exception e)
            {
                throw new Exception("The format of cleaned data reverse file is wrong.", e);
            }
        }

        private List<string> ParseAllGenome(StreamReader reader)
        {
            var genomes = new List<string>();

            SkipInfoLines(reader);

            var line = reader.ReadLine();

            while (line != null)
            {
                var genome = ParseGenome(line);
                genomes.Add(genome);

                line = reader.ReadLine();
            }

            return genomes;
        }

        private string ParseGenome(string line)
        {
            var cleanedDataReverseLineArray = line.Split('\t');
            var genome = cleanedDataReverseLineArray[0];

            return genome;
        }

        public Dictionary<string, List<uint>> ReadAllGenomeForGenes()
        {
            try
            {
                using (var cleanedDataReverseFileReader = new FileStream(Settings.Instance.GenesForGenomesFile, FileMode.Open))
                using (var cleanedDataReverseStreamReader = new StreamReader(cleanedDataReverseFileReader))
                {
                    return ParseAll(cleanedDataReverseStreamReader);
                }
            }
            catch (Exception e)
            {
                throw new Exception("The format of cleaned data reverse file is wrong.", e);
            }
        }

        private Dictionary<string, List<uint>> ParseAll(StreamReader reader)
        {
            var genesForGenomes = new Dictionary<string, List<uint>>();

            SkipInfoLines(reader);

            var line = reader.ReadLine();

            while (line != null)
            {
                var genesForGenomePair = ParseGenesForGenomesPair(line);
                genesForGenomes.Add(genesForGenomePair.Key, genesForGenomePair.Value);

                line = reader.ReadLine();
            }

            return genesForGenomes;
        }

        private KeyValuePair<string, List<uint>> ParseGenesForGenomesPair(string line)
        {
            var cleanedDataReverseLineArray = line.Split('\t');
            var genomeName = cleanedDataReverseLineArray[0];
            var genes = cleanedDataReverseLineArray[2].Split(' ').Select(uint.Parse).ToList();

            return new KeyValuePair<string, List<uint>>(genomeName, genes);
        }

        private void SkipInfoLines(TextReader reader)
        {
            reader.ReadLine();
            reader.ReadLine();
        }
    }
}