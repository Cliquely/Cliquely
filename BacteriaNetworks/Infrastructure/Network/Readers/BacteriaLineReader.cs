using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace BacteriaNetworks.Infrastructure.Network.Readers
{
	public class BacteriaLineReader
	{
		private string BacteriaLineFile { get; } = ConfigurationManager.AppSettings["bacteriaLineFile"];

		public List<Bacteria> ReadAllBacteria()
		{
			try
			{
				using (var cleanedDataReverseFileReader = new FileStream(BacteriaLineFile, FileMode.Open))
				using (var cleanedDataReverseStreamReader = new StreamReader(cleanedDataReverseFileReader))
				{
					return ParseAllBacteria(cleanedDataReverseStreamReader);
				}
			}
			catch (Exception e)
			{
				throw new Exception("The format of bacteria line file is wrong.", e);
			}
		}

		private List<Bacteria> ParseAllBacteria(StreamReader reader)
		{
			var bacterias = new List<Bacteria>();

			var line = reader.ReadLine();

			while (line != null)
			{
				var bacteria = ParseBacteria(line);
				bacterias.Add(bacteria);

				line = reader.ReadLine();
			}

			return bacterias;
		}

		private Bacteria ParseBacteria(string line)
		{
			var cleanedDataReverseLineArray = line.Split('\t');

			return new Bacteria
			{
				Abbr = cleanedDataReverseLineArray[0],
				Name = cleanedDataReverseLineArray[1]
			};
		}
	}
}