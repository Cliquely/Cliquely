using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliquely
{
    public static class ProbabilitiesCalculator
    {
        private static readonly string[] emptyChars = { " ", "\t" };

        public static Dictionary<uint, Dictionary<uint, float>> GetProbabilitiesForGene(uint i_Gene, float i_Probability, bool isHomology)
        {
            SqlHelper sql = new SqlHelper();
            Dictionary<uint, string[]> cleaned_data = new Dictionary<uint, string[]>();
            Dictionary<uint, int> genesToUse = new Dictionary<uint, int>();
            List<string> bacteriasForGene = new List<string>();
			Dictionary<string, List<uint>> reversed_cleaned_data = new Dictionary<string, List<uint>>();

			DataTable bacteriasForGeneTable = sql.Select("SELECT * FROM Bacteria WHERE Gene = "+i_Gene);
            foreach(DataRow row in bacteriasForGeneTable.Rows)
            {
                bacteriasForGene.Add(row.Field<string>("Bacteria"));
            }

            StringBuilder selectQuery = new StringBuilder("SELECT * FROM Bacteria WHERE Gene = " + i_Gene + " OR ");
            foreach (string bacteria in bacteriasForGene)
            {
                selectQuery.Append($"Bacteria = \"{bacteria}\" OR ");
            }
            selectQuery.Remove(selectQuery.Length - 3, 3);

            DataTable cleanDataTable = sql.Select(selectQuery.ToString());

            foreach (string bacteria in bacteriasForGene)
            {
                DataRow[] bacteriaRows = cleanDataTable.Select("Bacteria='" + bacteria.ToString() + "'");
                reversed_cleaned_data.Add(bacteria, getGenesForBacteria(bacteriaRows));
            }

            foreach (string bacteria in bacteriasForGene)
            {
                foreach(uint gene in reversed_cleaned_data[bacteria])
                {
                    if(!genesToUse.ContainsKey(gene))
                    {
                        genesToUse.Add(gene, 1);
                    }
                    else
                    {
                        genesToUse[gene] = genesToUse[gene] + 1;
                    }
                }
            }

            List<uint> toDelete = new List<uint>();
            foreach(KeyValuePair<uint, int> gene in genesToUse)
            {
                if(gene.Value < bacteriasForGene.Count * i_Probability)
                {
                    toDelete.Add(gene.Key);
                }
            }

            foreach(uint gene in toDelete)
            {
                genesToUse.Remove(gene);
            }

            foreach (uint i in genesToUse.Keys)
            {
                DataRow[] geneRows = cleanDataTable.Select("Gene=" + i.ToString());
                cleaned_data.Add(i, getBacteriasForGene(geneRows));
            }

            Dictionary<uint, string[]> genes = new Dictionary<uint, string[]>();
            foreach(uint gene in genesToUse.Keys)
            {
                genes.Add(gene, cleaned_data[gene]);
            }

			Dictionary<uint, float> geneDic = calculateProbabilitiesWithGenes(i_Gene, genes, i_Probability);
			toDelete.Clear();

			foreach (uint gene in genes.Keys)
			{
				if (!geneDic.ContainsKey(gene))
				{
					toDelete.Add(gene);
				}
			}

			foreach (uint gene in toDelete)
			{
				genesToUse.Remove(gene);
			}

			genes.Clear();

			foreach (uint gene in genesToUse.Keys)
			{
				genes.Add(gene, cleaned_data[gene]);
			}

			Dictionary<uint, Dictionary<uint, float>> probabilities = new Dictionary<uint, Dictionary<uint, float>>();

            if(isHomology)
            {
                Dictionary<uint, List<string>> homGenes = new Dictionary<uint, List<string>>();
                StringBuilder selectHomQuery = new StringBuilder("SELECT HomGene,Id FROM Gene WHERE ");
                int genesQueryCount = 0;

				foreach (uint gene in genes.Keys)
                {
                    selectHomQuery.Append($"Id = {gene} OR ");
                    genesQueryCount++;

                    if(genesQueryCount == 999)
                    {
                        selectHomQuery.Remove(selectHomQuery.Length - 3, 3);
                        DataTable homDataTable = sql.Select(selectHomQuery.ToString());

                        foreach (DataRow row in homDataTable.Rows)
                        {
                            uint geneId = (uint)row.Field<long>("Id");
                            uint homGeneId = (uint)row.Field<long>("HomGene");

                           if (homGenes.ContainsKey(homGeneId))
                            {
                                homGenes[homGeneId].AddRange(genes[geneId]);
                            }
                            else
                            {
                                homGenes.Add(homGeneId, genes[geneId].ToList());
                            }
                        }

                        selectHomQuery = new StringBuilder("SELECT HomGene,Id FROM Gene WHERE ");
                        genesQueryCount = 0;
                    }
                }

                genes.Clear();

                foreach(KeyValuePair<uint, List<string>> pair in homGenes)
                {
                    genes.Add(pair.Key, pair.Value.Distinct().ToArray());
                }
            }

            foreach (uint gene in genes.Keys)
            {
                Dictionary<uint, float> dic = calculateProbabilitiesWithGenes(gene, genes, i_Probability);
                if (dic.Count > 0)
                {
                    probabilities.Add(gene, dic);
                }
            }
            

            if (probabilities.Count > 0)
            {
                return probabilities;
            }

            return null;
        }

        private static Dictionary<uint, float> calculateProbabilitiesWithGenes(uint i_Gene, Dictionary<uint, string[]> i_BacteriasForGene, float i_TreshHoldProbability)
        {
            Dictionary<uint, float> probabilities = new Dictionary<uint, float>(i_BacteriasForGene.Count);
            foreach (uint j in i_BacteriasForGene.Keys)
            {
                if (i_Gene != j)
                {
                    float probability = calculateGeneProbability(i_BacteriasForGene[i_Gene], i_BacteriasForGene[j]);
                    if (probability >= i_TreshHoldProbability)
                    {
                        probabilities[j] = probability;
                    }
                }
            }

            return probabilities;
        }

        private static string[] getBacteriasForGene(DataRow[] i_Rows)
        {
            List<string> bacterias = new List<string>();
            foreach(DataRow row in i_Rows)
            {
                bacterias.Add(row["Bacteria"].ToString());
            }

            return bacterias.ToArray();
        }

        private static List<uint> getGenesForBacteria(DataRow[] i_Rows)
        {
            List<uint> genes = new List<uint>();
            foreach (DataRow row in i_Rows)
            {
                genes.Add(uint.Parse(row["Gene"].ToString()));
            }

            return genes.ToList();
        }

        private static float calculateGeneProbability(string[] i_BacteriasForGene1, string[] i_BacteriasForGene2)
        {
            int amountOfIntersectedBacterias = i_BacteriasForGene1.Intersect(i_BacteriasForGene2).Count();

            float numerator = amountOfIntersectedBacterias * amountOfIntersectedBacterias;
            float denominator = i_BacteriasForGene1.Length * i_BacteriasForGene2.Length;

            return numerator / denominator;
        }
    }
}