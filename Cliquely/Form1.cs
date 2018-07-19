using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BlastFromNCBI;

namespace Cliquely
{
	public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Blast.Finished += Blast_Finished;

			comboBoxGeneType.SelectedIndex = 0;
		}

        private void buttonSearchFasta_Click(object sender, EventArgs e)
        {
			genelnkLbl.Text = "Starts searching for a gene for the given fasta sequence.";
			genelnkLbl.LinkArea = new LinkArea(0, 0);
			CliquesDGV.DataSource = null;

			Blast.SendRequest(textBoxFasta.Text);
		}

        private void Blast_Finished(string i_RID, TimeSpan i_TimeSinceStarted, List<BlastGene> i_Genes)
        {
			if (i_Genes == null)
            {
				Invoke(new Action(() => { genelnkLbl.Text = "Could not find a gene for the given fasta sequence."; }));
				return;
            }
			
			SqlHelper sql = new SqlHelper();
			var blastGene = i_Genes.First();
			string selectQuery = $"SELECT GeneFasta.Gene, GeneFasta.Fasta FROM GeneFasta WHERE GeneFasta.Fasta = \"{blastGene.Sequence}\"";
			DataTable bacteriaTable = sql.Select(selectQuery.ToString());

            if(bacteriaTable.Rows.Count == 0)
            {
				Invoke(new Action(() => { genelnkLbl.Text = "Could not find a gene for the given fasta sequence."; }));
				return;
			}

            selectQuery = $"SELECT Gene.HomGene, Gene.Id, Gene.Details, GeneData.Bacteria FROM Gene INNER JOIN GeneData ON Gene.Id = GeneData.Gene WHERE GeneData.Bacteria = \"{bacteriaTable.Rows[0][0]}\"";

            DataTable geneTable = sql.Select(selectQuery.ToString());

			var gene = uint.Parse(geneTable.Rows[0][1].ToString());

			Invoke(new Action(() => {
				var geneDetails = getGeneLine(gene, true);
				genelnkLbl.Text = $"Gene: {geneDetails} was found most suitable for the given fasta sequence.\nStarts searching for cliques with the given gene.";

				genelnkLbl.LinkArea = new LinkArea(start: genelnkLbl.Text.IndexOf(':') + 2, length: geneDetails.ToString().Length + 1);
				genelnkLbl.LinkClicked += (sender, e) => System.Diagnostics.Process.Start($"https://www.ncbi.nlm.nih.gov/protein/{blastGene.AccessionId}");
			}));

			discoverCliques(gene);
        }

		private void discoverCliques(uint gene)
        {
			Dictionary<uint, Dictionary<uint, float>> probabilities;
			Dictionary<string, List<uint>> reversed_cleaned_data;

			var geneType = (string)Invoke(new Func<string>(() => comboBoxGeneType.SelectedItem.ToString()));

			if (geneType == "Homology")
			{
				probabilities = ProbabilitiesCalculator.GetProbabilitiesForGene(gene, float.Parse(textBoxTreshold.Text), true);// gets orthoGeneId and return hom genes probabilities.
				reversed_cleaned_data = getReversedCleanedDataHom(gene); // gets orthoGeneId
				gene = getHomGeneId(gene); // must be after 'getReversedCleanedDataHom(..)'
			}
			else
			{
				probabilities = ProbabilitiesCalculator.GetProbabilitiesForGene(gene, float.Parse(textBoxTreshold.Text), false);
				reversed_cleaned_data = getReversedCleanedDataOrtho(gene);
			}

			if (probabilities == null)
			{
				displayCouldNoFindAnyCliques(gene);
				return;
			}

            var discoverCliques = new DiscoverCliques(gene, probabilities);
            discoverCliques.Run();

            if (discoverCliques.Cliques.Count == 0)
            {
				displayCouldNoFindAnyCliques(gene);
				return;
			}

			Invoke(new Action(() => { genelnkLbl.Text = $"{geneFounded(gene)}\nDiscoverd all the cliques ({discoverCliques.Cliques.Count}) that containing the given gene (loading):"; }));

			if (discoverCliques.Cliques.Count <= 100)
			{
				displayCliquesInGridView(discoverCliques.Cliques, gene, reversed_cleaned_data);
			}
			else
			{
				exportCliquesToCSVFile(discoverCliques.Cliques, gene, reversed_cleaned_data);
			}

			Invoke(new Action(() => { genelnkLbl.Text = $"{geneFounded(gene)}\nDiscoverd all the cliques ({discoverCliques.Cliques.Count}) that containing the given gene:"; }));
		}

		private void displayCouldNoFindAnyCliques(uint gene)
		{
			Invoke(new Action(() => { genelnkLbl.Text = $"{geneFounded(gene)}\nCould not find any cliques for the given gene."; }));
		}

		private string geneFounded(uint gene)
		{
			var geneDetails = getGeneLine(gene);

			return $"Gene: {geneDetails} was found most suitable for the given fasta sequence.";
		}

		private void displayCliquesInGridView(List<List<uint>> cliques, uint gene, Dictionary<string, List<uint>> reversed_cleaned_data)
		{
			DataTable table = new DataTable();

			table.Columns.Add("Gene");
			table.Columns.Add("Probability");
			table.Columns.Add("Incidence");
			table.Columns.Add("Count");

			foreach (var clique in cliques)
			{
				var row = table.NewRow();
				var cliqueRowItems = getCliqueRowItems(clique, gene, reversed_cleaned_data);

				while (clique.Count > table.Columns.Count - 4)
				{
					table.Columns.Add("Gene " + (table.Columns.Count - 3));
				}

				row.ItemArray = cliqueRowItems.ToArray();
				table.Rows.Add(row);
			}

			this.Invoke(new Action(() =>
			{
				CliquesDGV.DataSource = table;
			}));
		}

		private List<string> getCliqueRowItems(List<uint> clique, uint gene, Dictionary<string, List<uint>> reversed_cleaned_data)
		{
			var cliqueRowItems = new List<string>();

			clique.Sort();
			int incidence = calculateIncidence(clique, reversed_cleaned_data.Values.ToList());

			cliqueRowItems.Add(getGeneLine(gene));
			cliqueRowItems.Add(textBoxTreshold.Text);
			cliqueRowItems.Add(incidence.ToString());

			cliqueRowItems.Add(clique.Count.ToString());
			cliqueRowItems.AddRange(clique.Select(x => getGeneLine(x)));

			return cliqueRowItems;
		}

		private void exportCliquesToCSVFile(List<List<uint>> cliques, uint gene, Dictionary<string, List<uint>> reversed_cleaned_data)
		{
			var csv = new StringBuilder();

			csv.AppendLine("Gene, Probability, Incidence, Count");
			cliques.ForEach(clique => csv.AppendLine(string.Join(",", getCliqueRowItems(clique, gene, reversed_cleaned_data))));

			using (var writer = new StreamWriter("Cliques.csv"))
			{
				writer.Write(csv.ToString());
			}

			Invoke(new Action(() =>
			{
				genelnkLbl.Text = "Saves cliques to csv file";

				MessageBox.Show("Exported all cliques to Cliques.csv");
			}));
		}

		private int calculateIncidence(List<uint> i_CliqueVerticesIDs, List<List<uint>> i_CleanedDataReverse)
		{
			int count = 0;
			i_CliqueVerticesIDs.Sort();

			foreach (List<uint> bacteria in i_CleanedDataReverse)
			{
				if (bacteria.Count >= i_CliqueVerticesIDs.Count)
				{
					var intersection = IntersectSorted(i_CliqueVerticesIDs, bacteria).ToList();
					var f = i_CliqueVerticesIDs.Where(x => !intersection.Contains(x));
					var intersectionCount = intersection.Count;

					if (intersectionCount == i_CliqueVerticesIDs.Count)
					{
						count++;
						System.Diagnostics.Debug.Write((i_CleanedDataReverse.FindIndex((b) => b == bacteria) + 1) + ", ");
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
					int comparison = value1.CompareTo(value2);
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

		private static Dictionary<string, List<uint>> getReversedCleanedDataHom(uint geneId)
		{
			var sql = new SqlHelper();
			var homDataTable = sql.Select($"select  gene.homgene as `Gene`, bacteria.bacteria as `Bacteria` from gene join bacteria on gene.id = bacteria.gene where bacteria.bacteria in (SELECT bacteria FROM Bacteria WHERE Gene = {geneId}) order by gene.homgene");

			return getReversedCleanedData(homDataTable);
		}

		private static Dictionary<string, List<uint>> getReversedCleanedDataOrtho(uint geneId)
		{
			var sql = new SqlHelper();
			var orthoDataTable = sql.Select($"select gene.id as `Gene`, bacteria.bacteria as `Bacteria` from gene join bacteria on gene.id = bacteria.gene where bacteria.bacteria in (SELECT bacteria FROM Bacteria WHERE Gene = {geneId}) order by gene.id");

			return getReversedCleanedData(orthoDataTable);
		}

		private static Dictionary<string, List<uint>> getReversedCleanedData(DataTable i_DataTable)
		{
			var reversedCleanedData = new Dictionary<string, List<uint>>();

			foreach (DataRow row in i_DataTable.Rows)
			{
				var bacteria = row["Bacteria"].ToString();
				var gene = uint.Parse(row["Gene"].ToString());

				if (!reversedCleanedData.Keys.Contains(bacteria))
				{
					reversedCleanedData.Add(bacteria, new List<uint>());
				}

				reversedCleanedData[bacteria].Add(gene);
			}

			return reversedCleanedData.Select(x => new { key = x.Key, val = x.Value.Distinct().ToList() }).ToDictionary(x => x.key, x => x.val);
		}

		private string getGeneLine(uint id, bool enforceOrthology = false)
		{
			DataTable dataTable;
			var sql = new SqlHelper();
			var geneType = (string)Invoke(new Func<string>(() => comboBoxGeneType.SelectedItem.ToString()));

			if (enforceOrthology || geneType != "Homology")
			{
				dataTable = sql.Select($"select details from gene where id = {id}");
			}
			else
			{
				dataTable = sql.Select($"select details from gene where homgene = {id}");
			}

			return dataTable.Rows[0][0].ToString();
		}

		private uint getHomGeneId(uint ortoGeneId)
		{
			return uint.Parse(new SqlHelper().Select($"select homgene from gene where id = {ortoGeneId}").Rows[0][0].ToString());
		}
	}
}