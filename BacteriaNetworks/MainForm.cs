using BacteriaNetworks.Infrastructure;
using BacteriaNetworks.Infrastructure.Network.Readers;
using CefSharp.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BacteriaNetworks
{
    public partial class MainForm : Form
    {
        private const float MAXIMUM_PROBABILITY = 1f;
        private const float MINIMUM_PROBABILITY = 0.7f;
        private const float PROBABILITY_DIFFERENCE = 0.1f;

        private List<string> BacteriaAbbrList { get; }
        private List<Bacteria> BacteriaList { get; }

        private ChromiumWebBrowser NetworkViewer { get; set; }
        private string BaseAddress { get; }

        public MainForm()
        {
            InitializeComponent();

            BaseAddress = $"file:///{Directory.GetCurrentDirectory()}/NetworkVisualization/index.html";
            InitializeNetworkViewer(BaseAddress);

            var cleanedDataReverseReader = new CleanedDataReverseReader();
            BacteriaAbbrList = cleanedDataReverseReader.ReadAllBacteria();

            var bacteriaLineReader = new BacteriaLineReader();
            BacteriaList = bacteriaLineReader.ReadAllBacteria();

            cmbFilter.SelectedIndexChanged += CmbFilterOnSelectedIndexChanged;
            InitFilters();
            initProbabilities();

            lblInfo.Text = string.Empty;
        }

        private void InitializeNetworkViewer(string baseAddress)
        {
            NetworkViewer = new ChromiumWebBrowser(baseAddress)
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Location = new System.Drawing.Point(2, 211),
                Margin = new Padding(0),
                Size = new System.Drawing.Size(1182, 540),
                TabIndex = 7,
                UseWaitCursor = true,
            };

            Controls.Add(NetworkViewer);
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

        private void initProbabilities()
        {
            cmbProbabiity.DataSource = getBacteriaProbabilityOptions();
            cmbProbabiity.SelectedIndex = 0;
        }

        private List<BacteriaProbabilityOption> getBacteriaProbabilityOptions()
        {
            var probabilities = new List<BacteriaProbabilityOption>();

            for (var i = MINIMUM_PROBABILITY; i <= MAXIMUM_PROBABILITY; i += PROBABILITY_DIFFERENCE)
            {
                probabilities.Add(new BacteriaProbabilityOption($"{i * 100}%", i));
            }

            return probabilities;
        }

        private void btnSearchNetwork_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateInfo(string.Empty);

                var probability = (cmbProbabiity.SelectedItem as BacteriaProbabilityOption).Value;
                var bacteria = GetBacteria();
                var bacteriaNetworks = new Infrastructure.Network.BacteriaNetworks();
                var network = bacteriaNetworks.SearchNetworks(bacteria, probability);

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

                LoadNetwork(genes, network);
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

        private void LoadNetwork(List<Gene> genes, List<List<Gene>> cliques)
        {
            var jsonGenes = JsonConvert.SerializeObject(genes.Select(gene => gene.Id));
            var jsonCliques = JsonConvert.SerializeObject(cliques.Select(clique => clique.Select(gene => gene.Id)));
            var address = $"{BaseAddress}?genes={jsonGenes}&cliques={jsonCliques}";

            NetworkViewer.Load(address);
        }

        private void UpdateInfo(string msg)
        {
            Invoke(new Action(() => { lblInfo.Text = msg; }));
        }
    }
}