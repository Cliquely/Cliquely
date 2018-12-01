using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BacteriaNetworks.Infrastructure;
using CefSharp.WinForms;

namespace BacteriaNetworks
{
    public partial class NetworkViewer : UserControl
    {
	    private string BaseAddress { get; }

		private ChromiumWebBrowser Browser { get; set; }

		public NetworkViewer()
		{
			InitializeComponent();

			BaseAddress = $"file:///{Directory.GetCurrentDirectory()}/NetworkVisualization/index.html";

			InitializeWebBrowser(BaseAddress);
		}

	    public void LoadNetwork(List<Gene> genes, List<List<Gene>> cliques)
	    {
		    var jsonGenes = JsonConvert.SerializeObject(genes.Select(gene => gene.Id));
		    var jsonCliques = JsonConvert.SerializeObject(cliques.Select(clique => clique.Select(gene => gene.Id)));
		    var address = $"{BaseAddress}?genes={jsonGenes}&cliques={jsonCliques}";

		    Browser.Load(address);
	    }

	    private void InitializeWebBrowser(string address)
        {
	        Browser = new ChromiumWebBrowser(address)
	        {
		        Dock = DockStyle.Fill,
		        Location = new System.Drawing.Point(0, 0),
		        TabIndex = 0
	        };

	        Controls.Add(Browser);
        }
    }
}
