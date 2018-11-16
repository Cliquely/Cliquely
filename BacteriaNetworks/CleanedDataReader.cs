using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace BacteriaNetworks
{
	public class CleanedDataReader
	{
		private string CleanDataHomFile { get; } = ConfigurationManager.AppSettings["cleanDataHomFile"];

		public List<string> ReadBacteriaForProteinById(int id)
		{
			try
			{
				using (var cleanedDataFileReader = new FileStream(CleanDataHomFile, FileMode.Open))
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
					return ParseBacteriaForProteinsPair(line).Value;
				}

				line = reader.ReadLine();
			}

			return null;
		}

		private uint getGeneLineId(string line)
		{
			return uint.Parse(line.Split('\t')[0]);
		}

		public Dictionary<uint, List<string>> ReadAllBacteriaForProteins()
		{
			try
			{
				using (var cleanedDataFileReader = new FileStream(CleanDataHomFile, FileMode.Open))
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
			var bacteriasForProteins = new Dictionary<uint, List<string>>();

			SkipInfoLines(reader);

			var line = reader.ReadLine();

			while (line != null)
			{
				var bacteriaForProteinsPair = ParseBacteriaForProteinsPair(line);
				bacteriasForProteins.Add(bacteriaForProteinsPair.Key, bacteriaForProteinsPair.Value);

				line = reader.ReadLine();
			}

			return bacteriasForProteins;
		}

		private KeyValuePair<uint, List<string>> ParseBacteriaForProteinsPair(string line)
		{
			var cleanedDataLineArray = line.Split('\t');
			var proteinId = uint.Parse(cleanedDataLineArray[0]);
			var bacterias = cleanedDataLineArray[2].Split(' ').ToList();

			return new KeyValuePair<uint, List<string>>(proteinId, bacterias);
		}

		private void SkipInfoLines(TextReader reader)
		{
			reader.ReadLine();
			reader.ReadLine();
		}
	}
}