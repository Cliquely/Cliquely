using System.Collections.Generic;
using System.Text;
using Cliquely;

namespace BacteriaNetworks
{
	public class BacterialNetworkDbWriter
	{
		private SqlHelper SqlHelper { get; } = new SqlHelper("BacterialNetwork.data");

		public void Write(List<uint> proteins, Dictionary<uint, Dictionary<uint, float>> bacterialNetwork)
		{
			WriteProteins(proteins);
			WriteNetwork(bacterialNetwork);
		}

		private void WriteNetwork(Dictionary<uint, Dictionary<uint, float>> bacterialNetwork)
		{
			const string initQuery = "INSERT INTO `HomologyProteinsProbabilities`(`firstProteinId`,`secondProteinId`,`probability`) VALUES ";

			var queryBuilder = new StringBuilder(initQuery);
			var i = 0;

			foreach (var firstProtein in bacterialNetwork.Keys)
			{
				foreach (var secondProtein in bacterialNetwork[firstProtein].Keys)
				{
					var probability = bacterialNetwork[firstProtein][secondProtein];

					queryBuilder.Append($"({firstProtein},{secondProtein},{probability}),");
					i++;

					if (i == 1000)
					{
						ExecuteInsertIntoQueryBuilder(queryBuilder);

						queryBuilder.Clear();
						queryBuilder.Append(initQuery);
						i = 0;
					}
				}
			}
		}

		private void ExecuteInsertIntoQueryBuilder(StringBuilder queryBuilder)
		{
			SqlHelper.Edit(queryBuilder.Replace(',', ';', queryBuilder.Length - 1, 1).ToString());
		}

		public void WriteProteins(List<uint> proteins)
		{
			const string initQuery = "INSERT INTO `HomologyProteins`(`id`) VALUES ";
			var queryBuilder = new StringBuilder(initQuery);
			var i = 0;

			foreach (var protein in proteins)
			{
				queryBuilder.Append($"({protein}),");
				i++;

				if (i == 1000)
				{
					ExecuteInsertIntoQueryBuilder(queryBuilder);

					queryBuilder.Clear();
					queryBuilder.Append(initQuery);
					i = 0;
				}
			}
		}
	}
}