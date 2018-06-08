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
            if(i_Genes == null)
            {
                Application.Exit();
            }
            SqlHelper sql = new SqlHelper();
            BlastGene gene = i_Genes.First();
            string selectQuery = $"SELECT GeneFasta.Gene, GeneFasta.Fasta FROM GeneFasta WHERE GeneFasta.Fasta = \"{gene.Sequence}\"";

            DataTable bacteriaTable = sql.Select(selectQuery.ToString());
            if(bacteriaTable.Rows.Count == 0)
            {
                // No cliques were found
                Application.Exit();
            }

            selectQuery = $"SELECT Gene.HomGene, Gene.Id, Gene.Details, GeneData.Bacteria FROM Gene INNER JOIN GeneData ON Gene.Id = GeneData.Gene WHERE GeneData.Bacteria = \"{bacteriaTable.Rows[0][0]}\"";

            DataTable GeneTable = sql.Select(selectQuery.ToString());

            discoverCliques(uint.Parse(GeneTable.Rows[0][0].ToString()));
        }

        private void discoverCliques(uint gene)
        {
            DiscoverCliques discoverCliques;
            var probabilities = ProbabilitiesCalculator.GetProbabilitiesForGene(gene, float.Parse(textBoxTreshold.Text), false);
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