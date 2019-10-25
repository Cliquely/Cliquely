using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Cliquely;
using Cliquely.Exceptions;

namespace CliquelyNetwork
{
	public partial class Main : Form
	{
	    private readonly Dictionary<uint, string> GeneLines = new Dictionary<uint, string>();

		public Main()
		{
			InitializeComponent();

			textBoxTreshold.Text = "0.7";
			textBoxMaxCliqueSize.Text = "30";
			textBoxMaxCliques.Text = "30";
			geneLbl.Text = "";
		}

		private void buttonSearchFasta_Click(object sender, EventArgs e)
		{
			buttonSearchFasta.Enabled = false;
			new Thread(SearchNetwork).Start();
		}

		private void SearchNetwork()
		{
			CliquesDGV.DataSource = null;

			try
			{
				ShowInfoMsg("Searching for cliques..");
				DiscoverCliques();
			}
			catch (CliquesNotFoundException)
			{
				DisplayCouldNoFindAnyCliques();
			}
			catch (Exception e)
			{
				var f = e;
				ShowInfoMsg("Some error occured.");
			}

			Invoke(new Action(() => buttonSearchFasta.Enabled = true));
		}

		private void ShowInfoMsg(string msg)
		{
			Invoke(new Action(() =>
			{
				geneLbl.Text = msg;
			}));
		}

		private void DiscoverCliques()
		{
			var maximalCliqueSize = GetMaximalCliqueSize();
			var maxCliques = GetMaxCliques();
			var probability = float.Parse(textBoxTreshold.Text);

			var probabilitiesCalculator = new ProbabilitiesCalculator(probability);
			var probabilities = probabilitiesCalculator.GetProbabilities();

			if (probabilities == null)
			{
				throw new CliquesNotFoundException();
			}

			var sortedGenes = probabilities.Keys
				.OrderByDescending(v => probabilities[v].Count)
				.ThenByDescending(v => v).ToList();

			var discoverCliques = new DiscoverCliques(sortedGenes, probabilities, maximalCliqueSize, maxCliques);
			discoverCliques.Run();

			if (discoverCliques.Cliques.Count == 0)
			{
				throw new CliquesNotFoundException();
			}

			ShowInfoMsg($"Discoverd all the cliques ({discoverCliques.Cliques.Count}) for the given probability (loading):");

			if (discoverCliques.Cliques.Count <= 100)
			{
				DisplayCliquesInGridView(discoverCliques.Cliques, probabilitiesCalculator.ReversedCleanedData);
			}

			ExportCliquesToCsvFile(discoverCliques.Cliques, probabilitiesCalculator.ReversedCleanedData, probability);

			ShowInfoMsg($"Discoverd all the cliques ({discoverCliques.Cliques.Count}) for the given probability:");
		}

		private int GetMaxCliques()
		{
			if (!int.TryParse(textBoxMaxCliques.Text, out var maxCliques))
			{
				ShowInfoMsg($"Maximum cliques must be an positive integer: {maxCliques}.");
			}

			return maxCliques;
		}

		private int GetMaximalCliqueSize()
		{
			if (!int.TryParse(textBoxMaxCliqueSize.Text, out var maximalCliqueSize))
			{
				ShowInfoMsg($"Maximal clique size must be an positive integer: {maximalCliqueSize}.");
			}

			return maximalCliqueSize;
		}

		private void DisplayCouldNoFindAnyCliques()
		{
			ShowInfoMsg("Could not find any cliques for the given probability.");
		}

		private void DisplayCliquesInGridView(List<List<uint>> cliques, Dictionary<string, List<uint>> reversedCleanedData)
		{
			var table = new DataTable();

			table.Columns.Add("Gene");
			table.Columns.Add("Probability");
			table.Columns.Add("Incidence");
			table.Columns.Add("Count");

			foreach (var clique in cliques)
			{
				var row = table.NewRow();
				var cliqueRowItems = GetCliqueRowItems(clique, reversedCleanedData);

				while (clique.Count > table.Columns.Count - 4)
				{
					table.Columns.Add("Gene " + (table.Columns.Count - 3));
				}

				row.ItemArray = cliqueRowItems.ToArray();

				table.Rows.Add(row);
			}

			Invoke(new Action(() =>
			{
				CliquesDGV.DataSource = table;
			}));
		}

		private List<string> GetCliqueRowItems(List<uint> clique, Dictionary<string, List<uint>> reversedCleanedData)
		{
			var cliqueRowItems = new List<string>();

			clique.Sort();

			var incidence = CalculateIncidence(clique, reversedCleanedData.Values.ToList());

			cliqueRowItems.Add(textBoxTreshold.Text);
			cliqueRowItems.Add(incidence.ToString());

			cliqueRowItems.Add(clique.Count.ToString());
			cliqueRowItems.AddRange(clique.Select(x => GetGeneLine(x)));

			return cliqueRowItems;
		}

		private void ExportCliquesToCsvFile(List<List<uint>> cliques, Dictionary<string, List<uint>> reversedCleanedData, float probability)
		{
			var csv = new StringBuilder();
			csv.AppendLine("Gene, Probability, Incidence, Count");
			cliques.ForEach(clique => csv.AppendLine(string.Join(",", MakeCsvCompatible(GetCliqueRowItems(clique, reversedCleanedData)))));

			using (var writer = new StreamWriter($"Cliques {probability}.csv"))
			{
				writer.Write(csv.ToString());
			}

			ShowInfoMsg("Saves cliques to csv file");
			Invoke(new Action(() =>
			{
				MessageBox.Show("Exported all cliques to Cliques.csv");
			}));
		}

		private List<string> MakeCsvCompatible(List<string> items)
		{
			for (var i = 0; i < items.Count; i++)
			{
				items[i] = $"\"{items[i]}\"";
			}

			return items;
		}

		private int CalculateIncidence(List<uint> cliqueVerticesIDs, List<List<uint>> cleanedDataReverse)
		{
			var count = 0;
			cliqueVerticesIDs.Sort();

			foreach (var bacteria in cleanedDataReverse)
			{
				if (bacteria.Count >= cliqueVerticesIDs.Count)
				{
					var intersection = IntersectSorted(cliqueVerticesIDs, bacteria).ToList();
					var intersectionCount = intersection.Count;

					if (intersectionCount == cliqueVerticesIDs.Count)
					{
						count++;
					}
				}
			}

			return count;
		}

		private IEnumerable<uint> IntersectSorted(IEnumerable<uint> sequence1, IEnumerable<uint> sequence2)
		{
			using (var cursor1 = sequence1.GetEnumerator())
			using (var cursor2 = sequence2.GetEnumerator())
			{
				if (!cursor1.MoveNext() || !cursor2.MoveNext())
				{
					yield break;
				}
				var value1 = cursor1.Current;
				var value2 = cursor2.Current;

				while (true)
				{
					var comparison = value1.CompareTo(value2);
					if (comparison < 0)
					{
						if (!cursor1.MoveNext())
						{
							yield break;
						}
						value1 = cursor1.Current;
					}
					else if (comparison > 0)
					{
						if (!cursor2.MoveNext())
						{
							yield break;
						}
						value2 = cursor2.Current;
					}
					else
					{
						yield return value1;
						if (!cursor1.MoveNext() || !cursor2.MoveNext())
						{
							yield break;
						}
						value1 = cursor1.Current;
						value2 = cursor2.Current;
					}
				}
			}
		}

		private void CliquesDGV_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
		{
			e.Column.FillWeight = 10;
		}

		private string GetGeneLine(uint id)
		{
			if (!GeneLines.ContainsKey(id))
			{
				GeneLines.Add(id, GetGeneLineFromFile(id));
			}

			return GeneLines[id];
		}

        private string GetGeneLineFromFile(uint id)
        {
            var currentGeneId = 1;

            using (var geneLine = new StreamReader(new FileStream(ConfigurationManager.AppSettings["GeneLinePath"], FileMode.Open, FileAccess.Read)))
            {
                string line;

                while ((line = geneLine.ReadLine()) != null)
                {
                    var lineArray = line.Split('\t');

                    if (currentGeneId++ == id)
                    {
                        return line.Replace('\t', ' ').Trim();
                    }
                }
            }

            throw new Exception();
        }
    }
}
