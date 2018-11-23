using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace BacteriaNetworks
{
	public class CleanedDataReverseReader
	{
		private string CleanDataReverseHomFile { get; } = ConfigurationManager.AppSettings["cleanDataReverseHomFile"];

		public List<uint> ReadProteinsForBacteriaByName(string name)
		{
			try
			{
				using (var cleanedDataReverseFileReader = new FileStream(CleanDataReverseHomFile, FileMode.Open))
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
					return ParseProteinsForBacteriasPair(line).Value;
				}

				line = reader.ReadLine();
			}

			return new List<uint>();
		}

		private string getGeneLineName(string line)
		{
			return line.Split('\t')[0];
		}

		public Dictionary<string, List<uint>> ReadAllBacteriaForProteins()
		{
			try
			{
				using (var cleanedDataReverseFileReader = new FileStream(CleanDataReverseHomFile, FileMode.Open))
				using (var cleanedDataReverseStreamReader = new StreamReader(cleanedDataReverseFileReader))
				{
					return ParseAll(cleanedDataReverseStreamReader);
				}
			}
			catch (Exception e)
			{
				throw new Exception("The format of cleaned data file is wrong.", e);
			}
		}

		private Dictionary<string, List<uint>> ParseAll(StreamReader reader)
		{
			var proteinsForBacterias = new Dictionary<string, List<uint>>();

			SkipInfoLines(reader);

			var line = reader.ReadLine();

			while (line != null)
			{
				var proteinsForBacteriaPair = ParseProteinsForBacteriasPair(line);
				proteinsForBacterias.Add(proteinsForBacteriaPair.Key, proteinsForBacteriaPair.Value);

				line = reader.ReadLine();
			}

			return proteinsForBacterias;
		}

		private KeyValuePair<string, List<uint>> ParseProteinsForBacteriasPair(string line)
		{
			var cleanedDataLineArray = line.Split('\t');
			var bacteriaName = cleanedDataLineArray[0];
			var proteins = cleanedDataLineArray[2].Split(' ').Select(uint.Parse).ToList();

			return new KeyValuePair<string, List<uint>>(bacteriaName, proteins);
		}

		private void SkipInfoLines(TextReader reader)
		{
			reader.ReadLine();
			reader.ReadLine();
		}
	}
}