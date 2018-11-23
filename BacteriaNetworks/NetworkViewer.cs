using CefSharp.WinForms;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BacteriaNetworks
{
    public partial class NetworkViewer : UserControl
    {
        private ChromiumWebBrowser browser;

        public NetworkViewer(List<Gene> genes, List<List<Gene>> cliques)
        {
            InitializeComponent();
            var jsonGenes = JsonConvert.SerializeObject(genes.Select(gene => gene.Id));
            var jsonCliques = JsonConvert.SerializeObject(cliques.Select(clique => clique.Select(gene => gene.Id)));
            var address = $"file:///{Directory.GetCurrentDirectory()}/NetworkVisualization/index.html?genes={jsonGenes}&cliques={jsonCliques}";

            initializeWebBrowser(address);
        }

        private void initializeWebBrowser(string address)
        {
            browser = new ChromiumWebBrowser(address);
            browser.Dock = DockStyle.Fill;
            browser.Location = new System.Drawing.Point(0, 0);
            browser.TabIndex = 0;
            Controls.Add(browser);
        }
    }
}
