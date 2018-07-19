using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Cliquely
{
	public static class ProbabilitiesCalculator
    {
        private static readonly string[] emptyChars = { " ", "\t" };

        public static Dictionary<uint, Dictionary<uint, float>> GetProbabilitiesForGene(uint i_Gene, float i_Probability, bool isHomology)
		{
			var sql = new SqlHelper();
			var reversed_cleaned_data = new Dictionary<string, List<uint>>();
			var cleaned_data = new Dictionary<uint, string[]>();
			var genesToUse = new Dictionary<uint, int>();
			var bacteriasForGene = new List<string>();

			var bacteriasForGeneTable = sql.Select("SELECT * FROM Bacteria WHERE Gene = " + i_Gene);

			foreach (DataRow row in bacteriasForGeneTable.Rows)
			{
				bacteriasForGene.Add(row.Field<string>("Bacteria"));
			}

			var cleanDataTable = GetCleanedDataTable(i_Gene, sql, bacteriasForGene);

			foreach (string bacteria in bacteriasForGene)
			{
				var bacteriaRows = cleanDataTable.Select("Bacteria='" + bacteria.ToString() + "'");
				reversed_cleaned_data.Add(bacteria, getGenesForBacteria(bacteriaRows));
			}

			foreach (string bacteria in bacteriasForGene)
			{
				foreach (uint gene in reversed_cleaned_data[bacteria])
				{
					if (!genesToUse.ContainsKey(gene))
					{
						genesToUse.Add(gene, 1);
					}
					else
					{
						genesToUse[gene] = genesToUse[gene] + 1;
					}
				}
			}

			var toDelete = new List<uint>();

			foreach (var gene in genesToUse)
			{
				if (gene.Value < bacteriasForGene.Count * i_Probability)
				{
					toDelete.Add(gene.Key);
				}
			}

			foreach (var gene in toDelete)
			{
				genesToUse.Remove(gene);
			}

			foreach (var i in genesToUse.Keys)
			{
				var geneRows = cleanDataTable.Select("Gene=" + i.ToString());
				cleaned_data.Add(i, getBacteriasForGene(geneRows));
			}

			var potentialGenes = new Dictionary<uint, string[]>();

			foreach (uint gene in genesToUse.Keys)
			{
				potentialGenes.Add(gene, cleaned_data[gene]);
			}

			var geneDic = calculateProbabilitiesWithGenes(i_Gene, potentialGenes, i_Probability);
			toDelete.Clear();

			foreach (uint gene in potentialGenes.Keys)
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

			potentialGenes.Clear();

			foreach (uint gene in genesToUse.Keys)
			{
				potentialGenes.Add(gene, cleaned_data[gene]);
			}

			var probabilities = new Dictionary<uint, Dictionary<uint, float>>();

			if (isHomology)
			{
				potentialGenes = ConvertOrthoGenesToHomGenes(potentialGenes);
			}

			foreach (var gene in potentialGenes.Keys)
			{
				var dic = calculateProbabilitiesWithGenes(gene, potentialGenes, i_Probability);

				if (dic.Any())
				{
					probabilities.Add(gene, dic);
				}
			}

			if (probabilities.Any())
			{
				return probabilities;
			}

			return null;
		}

		private static DataTable GetCleanedDataTable(uint i_Gene, SqlHelper sql, List<string> bacteriasForGene)
		{
			var cleanDataTable = new DataTable();
			var selectQuery = new StringBuilder($"SELECT * FROM Bacteria WHERE Gene = {i_Gene } OR ");
			var i = 0;

			foreach (string bacteria in bacteriasForGene)
			{
				selectQuery.Append($"Bacteria = \"{bacteria}\" OR ");
				i++;

				if(i == 998)
				{
					selectQuery.Remove(selectQuery.Length - 3, 3);
					cleanDataTable.Merge(sql.Select(selectQuery.ToString()));

					selectQuery.Clear();
					selectQuery.Append($"SELECT * FROM Bacteria WHERE Gene = {i_Gene } OR ");
					i = 0;
				}
			}

			if (i > 0)
			{
				selectQuery.Remove(selectQuery.Length - 3, 3);
				cleanDataTable.Merge(sql.Select(selectQuery.ToString()));
			}

			return cleanDataTable;
		}

		private static Dictionary<uint, string[]> ConvertOrthoGenesToHomGenes(Dictionary<uint, string[]> potentialGenes)
		{
			var homGenes = new Dictionary<uint, List<string>>();
			var selectHomQuery = new StringBuilder("SELECT HomGene,Id FROM Gene WHERE ");
			var sql = new SqlHelper();
			var genesQueryCount = 0;
			DataTable homDataTable;

			foreach (var gene in potentialGenes.Keys)
			{
				selectHomQuery.Append($"Id = {gene} OR ");
				genesQueryCount++;

				if (genesQueryCount == 999)
				{
					selectHomQuery.Remove(selectHomQuery.Length - 3, 3);
					homDataTable = sql.Select(selectHomQuery.ToString());

					foreach (DataRow row in homDataTable.Rows)
					{
						uint geneId = (uint)row.Field<long>("Id");
						uint homGeneId = (uint)row.Field<long>("HomGene");

						if (homGenes.ContainsKey(homGeneId))
						{
							homGenes[homGeneId].AddRange(potentialGenes[geneId]);
						}
						else
						{
							homGenes.Add(homGeneId, potentialGenes[geneId].ToList());
						}
					}

					selectHomQuery = new StringBuilder("SELECT HomGene,Id FROM Gene WHERE ");
					genesQueryCount = 0;
				}
			}

			if (genesQueryCount > 0)
			{
				selectHomQuery.Remove(selectHomQuery.Length - 3, 3);
				homDataTable = sql.Select(selectHomQuery.ToString());

				foreach (DataRow row in homDataTable.Rows)
				{
					uint geneId = (uint)row.Field<long>("Id");
					uint homGeneId = (uint)row.Field<long>("HomGene");

					if (homGenes.ContainsKey(homGeneId))
					{
						homGenes[homGeneId].AddRange(potentialGenes[geneId]);
					}
					else
					{
						homGenes.Add(homGeneId, potentialGenes[geneId].ToList());
					}
				}
			}

			var potentialHomGenes = new Dictionary<uint, string[]>();

			foreach (KeyValuePair<uint, List<string>> pair in homGenes)
			{
				potentialHomGenes.Add(pair.Key, pair.Value.Distinct().ToArray());
			}

			return potentialHomGenes;
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