using CliquesForGenome.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CliquesForGenome.Network.Readers
{
    public class CleanedDataReader
	{
		public List<string> ReadGenomeForGeneById(int id)
		{
			try
			{
				using (var cleanedDataFileReader = new FileStream(Settings.Instance.GenomesForGenesFile, FileMode.Open))
				using (var cleanedDataStreamReader = new StreamReader(cleanedDataFileReader))
				{
					return ParseOne(cleanedDataStreamReader, id);
				}
			}
			catch (Exception e)
			{
				throw new Exception("The format of cleaned data file is wrong.", e);
			}
		}

		private List<string> ParseOne(StreamReader reader, int id)
		{
			SkipInfoLines(reader);

			var line = reader.ReadLine();

			while (line != null)
			{
				var lineId = getGeneLineId(line);

				if (lineId == id)
				{
					return ParseGenomeForGenesPair(line).Value;
				}

				line = reader.ReadLine();
			}

			return null;
		}

		private uint getGeneLineId(string line)
		{
			return uint.Parse(line.Split('\t')[0]);
		}

		public Dictionary<uint, List<string>> ReadAllGenomeForGenes()
		{
			try
			{
				using (var cleanedDataFileReader = new FileStream(Settings.Instance.GenomesForGenesFile, FileMode.Open))
				using (var cleanedDataStreamReader = new StreamReader(cleanedDataFileReader))
				{
					return ParseAll(cleanedDataStreamReader);
				}
			}
			catch (Exception e)
			{
				throw new Exception("The format of cleaned data file is wrong.", e);
			}
		}

		private Dictionary<uint, List<string>> ParseAll(StreamReader reader)
		{
			var genomesForGenes = new Dictionary<uint, List<string>>();

			SkipInfoLines(reader);

			var line = reader.ReadLine();

			while (line != null)
			{
				var genomeForGenesPair = ParseGenomeForGenesPair(line);
				genomesForGenes.Add(genomeForGenesPair.Key, genomeForGenesPair.Value);

				line = reader.ReadLine();
			}

			return genomesForGenes;
		}

		private KeyValuePair<uint, List<string>> ParseGenomeForGenesPair(string line)
		{
			var cleanedDataLineArray = line.Split('\t');
			var geneId = uint.Parse(cleanedDataLineArray[0]);
			var genomes = cleanedDataLineArray[2].Split(' ').ToList();

			return new KeyValuePair<uint, List<string>>(geneId, genomes);
		}

		private void SkipInfoLines(TextReader reader)
		{
			reader.ReadLine();
			reader.ReadLine();
		}
	}
}