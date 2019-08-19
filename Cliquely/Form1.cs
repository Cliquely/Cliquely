using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cliquely.Exceptions;

namespace Cliquely
{
	public partial class Form1 : Form
    {
	    private Stopwatch Stopwatch { get; } = new Stopwatch();
	    private readonly Dictionary<uint, string> GeneLines = new Dictionary<uint, string>();

		public Form1()
        {
            InitializeComponent();

			comboBoxGeneType.SelectedIndex = 0;
	        textBoxTreshold.Text = "0.7";
	        textBoxMaxCliqueSize.Text = "30";
	        textBoxMaxCliques.Text = "30";
        }

        private void buttonSearchFasta_Click(object sender, EventArgs e)
        {
	        Stopwatch.Start();

			CliquesDGV.DataSource = null;

	        try
	        {
		        var gene = SearchGeneByFasteLine(textBoxFasta.Text);
		        DiscoverCliques(gene);
	        }
	        catch (GeneNotFoundException)
	        {
		        ShowInfoMsg("Could not find a gene for the given fasta sequence and the requirements.");
	        }
	        catch (CliquesNotFoundException ex)
	        {
		        DisplayCouldNoFindAnyCliques(ex.Gene);
	        }
        }

	    private uint SearchGeneByFasteLine(string fasta)
	    {
		    using (var fastaLine = new StreamReader(new FileStream(ConfigurationManager.AppSettings["fastaLinePath"], FileMode.Open, FileAccess.Read)))
		    {
			    string line;

			    while ((line = fastaLine.ReadLine()) != null)
			    {
					var lineArray = line.Split('\t');
					var currentFasta = lineArray[0];
					var genes = lineArray.Skip(1);

				    if (currentFasta == fasta)
				    {
						return uint.Parse(genes.First());
				    }
			    }
		    }

		    throw new GeneNotFoundException();
	    }

	    private void ShowInfoMsg(string msg)
	    {
		    Invoke(new Action(() =>
		    {
			    geneLbl.Text = msg;
		    }));
	    }

	    private void DiscoverCliques(uint gene)
        {
	        var maximalCliqueSize = GetMaximalCliqueSize();
	        var maxCliques = GetMaxCliques();

			var probabilitiesCalculator = new ProbabilitiesCalculator(gene, float.Parse(textBoxTreshold.Text));
			var probabilities = probabilitiesCalculator.GetProbabilities();

			if (probabilities == null)
			{
				throw new CliquesNotFoundException(gene);
			}

	        var sortedGenes = probabilities.Keys
		        .OrderByDescending(v => probabilities[v].Count)
		        .ThenByDescending(v => probabilitiesCalculator.GeneNeighboursProbabilities[v])
		        .ThenByDescending(v => v).ToList();

			var discoverCliques = new DiscoverCliques(gene, sortedGenes, probabilities, maximalCliqueSize, maxCliques);
			discoverCliques.Run();

            if (discoverCliques.Cliques.Count == 0)
            {
	            throw new CliquesNotFoundException(gene);
			}

			ShowInfoMsg($"{GeneFounded(gene)}\nDiscoverd all the cliques ({discoverCliques.Cliques.Count}) that containing the given gene (loading):");

			if (discoverCliques.Cliques.Count <= 100)
			{
				DisplayCliquesInGridView(discoverCliques.Cliques, gene, probabilitiesCalculator.ReversedCleanedData);
			}

            ExportCliquesToCsvFile(discoverCliques.Cliques, gene, probabilitiesCalculator.ReversedCleanedData);

			ShowInfoMsg($"{GeneFounded(gene)}\nDiscoverd all the cliques ({discoverCliques.Cliques.Count}) that containing the given gene:");
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

	    private void DisplayCouldNoFindAnyCliques(uint gene)
		{
			ShowInfoMsg($"{GeneFounded(gene)}\nCould not find any cliques for the given gene.");
		}

		private string GeneFounded(uint gene)
		{
			var geneDetails = GetGeneLine(gene);

			return $"Gene: {geneDetails} was found most suitable for the given fasta sequence.";
		}

		private void DisplayCliquesInGridView(List<List<uint>> cliques, uint gene, Dictionary<string, List<uint>> reversedCleanedData)
		{
			var table = new DataTable();

			table.Columns.Add("Gene");
			table.Columns.Add("Probability");
			table.Columns.Add("Incidence");
			table.Columns.Add("Count");

			foreach (var clique in cliques)
            {
                var row = table.NewRow();
                var cliqueRowItems = GetCliqueRowItems(clique, gene, reversedCleanedData);

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

        private List<string> GetCliqueRowItems(List<uint> clique, uint gene, Dictionary<string, List<uint>> reversedCleanedData)
		{
			var cliqueRowItems = new List<string>();

			clique.Sort();

			var incidence = CalculateIncidence(clique, reversedCleanedData.Values.ToList());

            cliqueRowItems.Add(GetGeneLine(gene));
            cliqueRowItems.Add(textBoxTreshold.Text);
			cliqueRowItems.Add(incidence.ToString());

			cliqueRowItems.Add(clique.Count.ToString());
			cliqueRowItems.AddRange(clique.Select(x => GetGeneLine(x)));

			return cliqueRowItems;
		}

		private void ExportCliquesToCsvFile(List<List<uint>> cliques, uint gene, Dictionary<string, List<uint>> reversedCleanedData)
		{
			var csv = new StringBuilder();
			csv.AppendLine("Gene, Probability, Incidence, Count");
			cliques.ForEach(clique => csv.AppendLine(string.Join(",", MakeCsvCompatible(GetCliqueRowItems(clique, gene, reversedCleanedData)))));

			using (var writer = new StreamWriter($"Cliques {gene}.csv"))
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
					var f = cliqueVerticesIDs.Where(x => !intersection.Contains(x));
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
		    var idStr = id.ToString();

		    using (var geneLine = new StreamReader(new FileStream(ConfigurationManager.AppSettings["GeneLinePath"], FileMode.Open, FileAccess.Read)))
		    {
			    string line;

			    while ((line = geneLine.ReadLine()) != null)
			    {
				    var lineArray = line.Split('\t');
				    var currentGeneId = lineArray[0];

				    if (currentGeneId == idStr)
				    {
					    return line;
				    }
			    }
		    }

		    throw new Exception();
		}
	}
}