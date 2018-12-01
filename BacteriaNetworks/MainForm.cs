using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BacteriaNetworks.Infrastructure;
using BacteriaNetworks.Infrastructure.Network.Readers;

namespace BacteriaNetworks
{
	public partial class MainForm : Form
	{
		private List<string> BacteriaAbbrList { get; }
		private List<Bacteria> BacteriaList { get; }

		public MainForm()
		{
			InitializeComponent();

			var cleanedDataReverseReader = new CleanedDataReverseReader();
			BacteriaAbbrList = cleanedDataReverseReader.ReadAllBacteria();

			var bacteriaLineReader = new BacteriaLineReader();
			BacteriaList = bacteriaLineReader.ReadAllBacteria();

			cmbFilter.SelectedIndexChanged += CmbFilterOnSelectedIndexChanged;
			InitFilters();

			lblInfo.Text = string.Empty;
		}

		private void CmbFilterOnSelectedIndexChanged(object sender, EventArgs e)
		{
			var val = cmbFilter.SelectedItem.ToString();

			if (val == BacteriaFilterType.Abbr.GetDescription())
			{
				LoadCmbBacteriaData(BacteriaAbbrList);
			}
			else if (val == BacteriaFilterType.FullName.GetDescription())
			{
				LoadCmbBacteriaData(BacteriaList.Select(x => x.Name).ToList());
			}
		}

		private void LoadCmbBacteriaData(List<string> data)
		{
			cmbBacteria.Data = data;
			cmbBacteria.Items.AddRange(data.ToArray());
		}

		private void InitFilters()
		{
			cmbFilter.DataSource = Enum.GetValues(typeof(BacteriaFilterType))
				.Cast<BacteriaFilterType>()
				.Select(x => x.GetDescription())
				.ToList();

			cmbFilter.SelectedIndex = 0;
		}

		private void btnSearchNetwork_Click(object sender, EventArgs e)
		{
			try
			{
				UpdateInfo(string.Empty);

				var bacteria = GetBacteria();
				var bacteriaNetworks = new Infrastructure.Network.BacteriaNetworks();
				var network = bacteriaNetworks.SearchNetworks(bacteria, 0.7f);

				if (!network.Any())
				{
					UpdateInfo("No cliques were found...");
					return;
				}

				var genes = network.Aggregate(
					new List<Gene>(), 
					(distinctGenes, currentGenes) =>
					{
						distinctGenes.AddRange(currentGenes);
						return distinctGenes;
					}).Distinct().ToList();

				networkViewer1.LoadNetwork(genes, network);
			}
			catch (ArgumentException)
			{
				UpdateInfo("Invalid bacteria name, please try again...");
			}
		}

		private string GetBacteria()
		{
			var bacteria = cmbBacteria.SelectedItem.ToString();

			if (cmbFilter.SelectedItem.ToString() == BacteriaFilterType.Abbr.GetDescription())
			{
				if (BacteriaAbbrList.Contains(bacteria))
				{
					return bacteria;
				}
			}
			else if (cmbFilter.SelectedItem.ToString() == BacteriaFilterType.FullName.GetDescription())
			{
				var bacteriaAbbr = BacteriaList.FirstOrDefault(x => x.Name == bacteria);

				if (bacteriaAbbr != null)
				{
					return bacteriaAbbr.Abbr;
				}
			}

			throw new ArgumentException();
		}

		private void UpdateInfo(string msg)
		{
			Invoke(new Action(() => { lblInfo.Text = msg; }));
		}
	}
}