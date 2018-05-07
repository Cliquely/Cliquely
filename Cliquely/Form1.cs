using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private void buttonSearchFasta_Click(object sender, EventArgs e)
        {
            Blast.SendRequest(textBoxFasta.Text);
        }

        private void Blast_Finished(string i_RID, TimeSpan i_TimeSinceStarted, List<BlastGene> i_Genes)
        {
            SqlHelper sql = new SqlHelper();
            StringBuilder selectQuery = new StringBuilder("SELECT GeneFasta.Gene, GeneFasta.Fasta FROM GeneFasta WHERE ");
            foreach(BlastGene gene in i_Genes.Where(x => x.MatchingPercentage > 50))
            {
                selectQuery.Append($"GeneFasta.Fasta = \"{gene.Sequence}\" OR ");
            }
            selectQuery.Remove(selectQuery.Length - 3, 3);
            DataTable bacteriaTable = sql.Select(selectQuery.ToString());

            selectQuery.Clear();

            selectQuery.Append("SELECT Gene.HomGene, Gene.Id, Gene.Details, GeneData.Bacteria FROM Gene INNER JOIN GeneData ON Gene.Id = GeneData.Gene WHERE ");
            foreach (DataRow bacteria in bacteriaTable.Rows)
            {
                selectQuery.Append($"GeneData.Bacteria = \"{bacteria[0]}\" OR ");
            }
            selectQuery.Remove(selectQuery.Length - 3, 3);
            selectQuery.Append("ORDER BY Gene.Id");
            DataTable GeneTable = sql.Select(selectQuery.ToString());

            selectQuery.Clear();

            DataTable finalTable = new DataTable();
            finalTable.Columns.Add("HomClusterID");
            finalTable.Columns.Add("Id");
            finalTable.Columns.Add("Info");
            finalTable.Columns.Add("Gene");
            finalTable.Columns.Add("Match %");
            finalTable.Columns["Match %"].DataType = typeof(float);
            finalTable.Columns.Add("Fasta");

            var distinctGenes = GeneTable.AsEnumerable().Select(row => row.Field<long>("Id")).Distinct();

            foreach(long geneId in distinctGenes)
            {
                long HomGene = GeneTable.AsEnumerable().Where(row => row.Field<long>("Id") == geneId).First().Field<long>("HomGene");
                var bacterias = GeneTable.Select("Id = " + geneId).Select(row => row["Bacteria"]);
                var BestMatchingBacteria = bacteriaTable.AsEnumerable().Where(row => bacterias.Contains(row["Gene"])).OrderByDescending(row => i_Genes.Single(x=> x.Sequence == row.Field<string>("Fasta")).MatchingPercentage).Select(row => new { Bacteria = row.Field<string>("Gene"), Fasta = row.Field<string>("Fasta")}).First();
                float matchingPercentage = i_Genes.Where(gene => gene.Sequence == BestMatchingBacteria.Fasta).Select(gene => gene.MatchingPercentage).Single();
                string geneInfo = GeneTable.AsEnumerable().Where(row => row.Field<long>("Id") == geneId).First().Field<string>("Details");
                finalTable.Rows.Add(HomGene, geneId, geneInfo, BestMatchingBacteria.Bacteria, matchingPercentage, BestMatchingBacteria.Fasta);
            }

            finalTable.DefaultView.Sort = "Match % DESC";

            dataGridView1.Invoke(new MethodInvoker(() => { dataGridView1.DataSource = finalTable; }));
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DiscoverCliques discoverCliques;
            uint gene = uint.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            var probabilities = ProbabilitiesCalculator.GetProbabilitiesForGene(gene, float.Parse(textBoxTreshold.Text));
            if (probabilities == null)
            {
                return;
            }

            discoverCliques = new DiscoverCliques(gene, probabilities);
            discoverCliques.Run();

            if (discoverCliques.Cliques.Count == 0)
            {
                return;
            }
        }
    }
}