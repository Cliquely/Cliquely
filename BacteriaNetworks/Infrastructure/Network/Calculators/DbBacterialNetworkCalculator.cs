using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Cliquely;

namespace BacteriaNetworks.Infrastructure.Network.Calculators
{
	public class DbBacterialNetworkCalculator : BacterialNetworkCalculator, IDisposable
	{
		public const string DefaultDB = "BacterialNetwork.data";

		private BulkSqlHelper SqlHelper { get; } = new BulkSqlHelper(DefaultDB);
		private Dictionary<uint, float> CurrentGeneNeighbors { get; } = new Dictionary<uint, float>();

		public Dictionary<uint, Dictionary<uint, float>> CalculateForOneBacteria(List<uint> proteinsInBacteria)
		{
			return Calculate(proteinsInBacteria);
		}

		public Dictionary<uint, Dictionary<uint, float>> CalculateAll(List<uint> allProteins)
		{
			return Calculate(allProteins);
		}

		public List<uint> GetGenes()
		{
			const string query = "SELECT * FROM HomologyProteins";
			var genes = ConvertToGeneList(SqlHelper.Select(query));

			return genes.ToList();
		}

		private IEnumerable<uint> ConvertToGeneList(DataTable genesDataTable)
		{
			return genesDataTable.Rows.Cast<DataRow>().Select(x => uint.Parse(x[0].ToString()));
		}

		protected override Dictionary<uint, Dictionary<uint, float>> Calculate(List<uint> proteins)
		{
			var bacterialNetwork = new Dictionary<uint, Dictionary<uint, float>>();

			foreach (var firstProteinId in proteins)
			{
				UpdateCurrentGeneNeighbors(firstProteinId);

				foreach (var secondProteinId in proteins)
				{
					var proteinsProbability = GetProbability(firstProteinId, secondProteinId);

					AddProteinsProbabilityToNetwork(bacterialNetwork, proteinsProbability, firstProteinId, secondProteinId);
				}
			}

			return bacterialNetwork;
		}

		private void UpdateCurrentGeneNeighbors(uint firstProteinId)
		{
			var query =
$@"select firstProteinId as protein, probability from HomologyProteinsProbabilities where secondProteinId = {firstProteinId}
union
select secondProteinId as protein, probability from HomologyProteinsProbabilities where firstProteinId = {firstProteinId}";

			CurrentGeneNeighbors.Clear();
			SqlHelper.Select(query).Rows.Cast<DataRow>().ToList().ForEach(AddToCurrentGeneNeighbors);
		}

		private void AddToCurrentGeneNeighbors(DataRow x)
		{
			CurrentGeneNeighbors.Add(uint.Parse(x[0].ToString()), float.Parse(x[1].ToString()));
		}

		protected override float GetProbability(uint firstProteinId, uint secondProteinId)
		{
			if (CurrentGeneNeighbors.ContainsKey(secondProteinId)) return CurrentGeneNeighbors[secondProteinId];

			return ThresholdProbability - 1;
		}

		/*protected override float GetProbability(uint firstProteinId, uint secondProteinId)
		{
			var query =
$@"select probability 
from HomologyProteinsProbabilities 
where (firstProteinId = {firstProteinId} and secondProteinId = {secondProteinId}) or 
(firstProteinId = {secondProteinId} and secondProteinId = {firstProteinId})";

			var genesProbabilityRows = SqlHelper.Select(query).Rows;

			if (genesProbabilityRows.Count == 0) return ThresholdProbability - 1;

			return float.Parse(genesProbabilityRows[0][0].ToString());
		}*/

		public void Dispose()
		{
			SqlHelper?.Dispose();
		}
	}
}