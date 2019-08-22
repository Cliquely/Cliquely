using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
	internal class Program
	{
		private const string GENE_LINE_SRC_FILE = @"D:\ProteinFamilies\CleanData\bin\Debug\gene_line.txt";
		private const string SRC_FILE = @"D:\ProteinFamilies\FastaToGene\bin\Debug\fasta_line1.txt";
		private const string DST_FILE = @"D:\ProteinFamilies\FastaToGene\bin\Debug\genesToFastaIds1.txt";

		private static Dictionary<string, List<uint>> AliasToIds { get; set; }

		public static void Main()
		{
			AliasToIds = GetAliasToIds();
			var genesToFasta = GetGenesToFasta();
			WriteGenesToFastaToFile(genesToFasta);
		}

		private static Dictionary<string, List<uint>> GetAliasToIds()
		{
			var aliasToIds = new Dictionary<string, List<uint>>();

			using (var srcFile = new StreamReader(new FileStream(GENE_LINE_SRC_FILE, FileMode.Open, FileAccess.Read)))
			{
				string line;

				while ((line = srcFile.ReadLine()) != null)
				{
					var lineArray = line.Split("\t");
					var id = uint.Parse(lineArray.First());
					var aliasesSection = Regex.Replace(lineArray.Last(), "\\(.*\\)+", "");
					var aliases = aliasesSection.Split(" ");

					foreach (var alias in aliases)
					{
						if (!aliasToIds.ContainsKey(alias))
						{
							aliasToIds[alias] = new List<uint>();
						}

						aliasToIds[alias].Add(id);
					}
				}
			}

			return aliasToIds;
		}

		private static void WriteGenesToFastaToFile(SortedDictionary<string, List<uint>> genesToFasta)
		{
			using (var dstFile = new StreamWriter(new FileStream(DST_FILE, FileMode.Create, FileAccess.Write)))
			{
				foreach (var fasta in genesToFasta.Keys)
				{
					var genes = string.Join("\t", genesToFasta[fasta]);

					dstFile.WriteLine($"{fasta}\t{genes}");
				}
			}
		}

		private static SortedDictionary<string, List<uint>> GetGenesToFasta()
		{
			var genesToFasta = new SortedDictionary<string, List<uint>>();

			#if DEBUG
			var i = 0;
			#endif

			using (var srcFile = new StreamReader(new FileStream(SRC_FILE, FileMode.Open, FileAccess.Read)))
			{
				string line;

				while ((line = srcFile.ReadLine()) != null)
				{
					var lineArray = line.Split("\t");
					var gene = lineArray[0];
					var fasta = lineArray[1];

					if (AliasToIds.ContainsKey(gene))
					{
						if (genesToFasta.ContainsKey(fasta) == false)
						{
							genesToFasta.Add(fasta, new List<uint>());
						}

						genesToFasta[fasta].AddRange(AliasToIds[gene]);
					}

					#if DEBUG
					i++;

					if (i % 500000 == 0)
					{
						Console.WriteLine(i/500000);
					}
					 #endif
				}
			}

			return genesToFasta;
		}
	}
}
