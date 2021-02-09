using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BlastFromNCBI;
using Cliquely.Exceptions;

namespace Cliquely
{
	public partial class Form1 : Form
	{
	    private readonly Dictionary<uint, string> GeneLines = new Dictionary<uint, string>();
	    private string LastFasta { get; set; }
	    private uint LastGene { get; set; }

	    public Form1()
        {
            InitializeComponent();

	        textBoxTreshold.Text = "0.7";
	        textBoxMaxCliqueSize.Text = "30";
	        textBoxMaxCliques.Text = "30";
	        geneLbl.Text = "";

	        comboBoxTaxonomy.DataSource = Enum.GetValues(typeof(eTaxonomy));
	        comboBoxTaxonomy.SelectedItem = eTaxonomy.All;

			Blast.Finished += BlastOnFinished;
		}

		private void BlastOnFinished(string i_rid, TimeSpan i_timesincestarted, List<BlastGene> i_genes)
		{
			if (i_genes == null) throw new GeneNotFoundException();

			var gene = SearchGene(i_genes.First().Sequence);

			if (gene == null)
			{
				ShowInfoMsg("Could not find a gene for the given fasta sequence and the requirements.");
			}

			ShowInfoMsg(GeneFounded(gene.Value));
			SearchCliques(gene.Value);
			Invoke(new Action(EnableUI));
		}

		private void buttonSearchFasta_Click(object sender, EventArgs e)
        {
	        textBoxFasta.ReadOnly = true;

	        buttonSearchFasta.Enabled = false;
	        textBoxMaxCliqueSize.Enabled = false;
	        textBoxMaxCliques.Enabled = false;
	        textBoxTreshold.Enabled = false;
	        comboBoxTaxonomy.Enabled = false;

	        CliquesDGV.DataSource = null;
	        ShowInfoMsg(string.Empty);

	        new Thread(Search).Start();
        }

		private void Search()
		{
			var fasta = textBoxFasta.Text;
			uint? gene;

			gene = fasta == LastFasta ? LastGene : SearchGene(fasta);

			if (gene != null)
			{
				LastFasta = fasta;
				LastGene = gene.Value;

				ShowInfoMsg(GeneFounded(gene.Value));
				SearchCliques(gene.Value);
				Invoke(new Action(EnableUI));

				return;
			}

			ShowInfoMsg("Could not find a gene for the given fasta sequence, searching in Blast...");

			Blast.SendRequest(fasta);
		}

		private void EnableUI()
		{
			textBoxFasta.ReadOnly = false;

			buttonSearchFasta.Enabled = true;
			textBoxMaxCliqueSize.Enabled = true;
			textBoxMaxCliques.Enabled = true;
			textBoxTreshold.Enabled = true;
			comboBoxTaxonomy.Enabled = true;
		}

		private uint? SearchGene(string fasta)
		{
			uint? gene = null;

			try
			{
				gene = SearchFastaInFastaLine(fasta);
			}
			catch (GeneNotFoundException)
			{
				ShowInfoMsg("Could not find a gene for the given fasta sequence and the requirements.");
			}
			catch (Exception e)
			{
				ShowInfoMsg("Some error occured.");
			}

			return gene;
		}

		private void SearchCliques(uint gene)
		{
			try
			{
				DiscoverCliques(gene);
			}
			catch (CliquesNotFoundException ex)
			{
				DisplayCouldNoFindAnyCliques(ex.Gene);
			}
			catch (Exception e)
			{
				ShowInfoMsg("Some error occured.");
			}
		}

		private uint? SearchFastaInFastaLine(string fasta)
	    {
			using (var fastaLine = new StreamReader(new FileStream(ConfigurationManager.AppSettings["fastaLinePath"],
			    FileMode.Open, FileAccess.Read)))
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

		    return null;
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

			var probability = float.Parse(textBoxTreshold.Text);
			eTaxonomy? taxonomy = null;

			Invoke(new Action(() =>
			{
				taxonomy = (eTaxonomy)Enum.Parse(typeof(eTaxonomy), comboBoxTaxonomy.SelectedItem.ToString());
			}));

			var probabilitiesCalculator = new GeneProbabilitiesCalculator(gene, probability, taxonomy.Value);
			var probabilities = probabilitiesCalculator.GetProbabilities();

			if (probabilities == null)
			{
				throw new CliquesNotFoundException { Gene = gene };
			}

			var sortedGenes = probabilities.Keys
				.OrderByDescending(v => probabilities[v].Count)
				.ThenByDescending(v => probabilitiesCalculator.GeneNeighboursProbabilities[v])
				.ThenByDescending(v => v).ToList();

			if(probability == 1)
			{
				ShowCliques(gene, probabilitiesCalculator.ReversedCleanedData, new List<List<uint>> { sortedGenes }, 0, taxonomy.Value);
				return;
			}

			var discoverCliques = new DiscoverCliquesByGene(gene, sortedGenes, probabilities, maximalCliqueSize, maxCliques);
			discoverCliques.Run();

			if (discoverCliques.Cliques.Count == 0)
			{
				throw new CliquesNotFoundException { Gene = gene };
			}

			ShowCliques(gene, probabilitiesCalculator.ReversedCleanedData, discoverCliques.Cliques, discoverCliques.AmountOfCliquesLargerThanMaxCliqueSize, taxonomy.Value);
		}

		private void ShowCliques(uint gene, Dictionary<string, List<uint>> reversedCleanedData, List<List<uint>> cliques, int amountOfDismissedCliquesBySize, eTaxonomy taxonomy)
		{
			var cliquesDiscoveredMsg = $"{GeneFounded(gene)}\nDiscoverd all the cliques ({cliques.Count}) that containing the given gene";
			var dismissedCliquesMsg = $"{amountOfDismissedCliquesBySize} were dismissed because they were too large";

			ShowInfoMsg($"{cliquesDiscoveredMsg}{(amountOfDismissedCliquesBySize > 0 ? $".\n{dismissedCliquesMsg}" : string.Empty)} loading:");

			if (cliques.Count <= 100)
			{
				DisplayCliquesInGridView(cliques, gene, reversedCleanedData);
			}

			ExportCliquesToCsvFile(cliques, gene, reversedCleanedData, taxonomy);

			ShowInfoMsg($"{cliquesDiscoveredMsg}{(amountOfDismissedCliquesBySize > 0 ? $".\n{dismissedCliquesMsg}" : string.Empty)}:");
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

		private void ExportCliquesToCsvFile(List<List<uint>> cliques, uint gene, Dictionary<string, List<uint>> reversedCleanedData, eTaxonomy taxonomy)
		{
			var csv = new StringBuilder();
			csv.AppendLine("Gene, Probability, Incidence, Count");
			cliques.ForEach(clique => csv.AppendLine(string.Join(",", MakeCsvCompatible(GetCliqueRowItems(clique, gene, reversedCleanedData)))));

			using (var writer = new StreamWriter($"Cliques {gene}{(taxonomy == eTaxonomy.All ? string.Empty : $"({taxonomy})")}.csv"))
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