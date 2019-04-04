using CliquesGraphs.Excel;
using System.Collections.Generic;
using System.Linq;

namespace CliquesGraphs.Graphs
{
    public class CliquesInGenomeGraph : GraphPlanner
    {
        private readonly eTaxonomy taxonomy;

        public CliquesInGenomeGraph(ExcelGraphBook excel, eTaxonomy taxonomy = eTaxonomy.Archaea | eTaxonomy.Bacteria | eTaxonomy.Eukaryota) : base(excel)
        {
            this.taxonomy = taxonomy;
        }

        protected override void runCalculations(Dictionary<string, List<CliqueRecord>> dataFiles)
        {
            foreach (var (name, data) in dataFiles)
            {
                var dataForTaxonomy = data.Where(clique => (clique.Taxonomy & taxonomy) > 0).ToList();
                var cliquesInGenome = calculateAmountOfCliquesInGenome(dataForTaxonomy);
                var cliquesSizeIncidence = calculateCliquesSizeIncidence(cliquesInGenome);
                var cliquesSizeIncidencePercentage = calculateCliquesSizeIncidencePercentage(cliquesSizeIncidence, cliquesInGenome.Count);

                if (worksheet["Amount Of Cliques"] == null || worksheet["Amount Of Cliques"]?.Items < cliquesSizeIncidence.Length)
                    worksheet.AddColumn("Amount Of Cliques", Enumerable.Range(1, cliquesSizeIncidence.Length).Where(i => cliquesSizeIncidence[i - 1] > 0));

                worksheet.AddColumn($"{name} Incidence", cliquesSizeIncidence.Where(i => i > 0));
                worksheet.AddColumn($"{name} percentage", cliquesSizeIncidencePercentage.Where(i => i > 0));
            }


            worksheet.AddGraph("Cliques Incidence", "Amount Of Cliques",
                dataFiles.Select(datafile => $"{datafile.Key} percentage").ToArray());
        }

        private Dictionary<string, int> calculateAmountOfCliquesInGenome(List<CliqueRecord> cliques)
        {
            var cliquesInGenome = new Dictionary<string, int>();

            foreach (var clique in cliques)
            {
                if (!cliquesInGenome.ContainsKey(clique.Abbrev))
                {
                    cliquesInGenome.Add(clique.Abbrev, 0);
                }

                cliquesInGenome[clique.Abbrev]++;
            }

            return cliquesInGenome;
        }

        private int[] calculateCliquesSizeIncidence(Dictionary<string, int> cliquesInGenome)
        {
            var maxCliquesInGenome = cliquesInGenome.Values.Max();
            var cliquesInGenomeCounter = new int[maxCliquesInGenome];

            foreach(var amountOfCliques in cliquesInGenome.Values)
            {
                cliquesInGenomeCounter[amountOfCliques - 1]++;
            }

            return cliquesInGenomeCounter;
        }

        private float[] calculateCliquesSizeIncidencePercentage(int[] cliquesSizeIncidence, int amountOfCliques)
        {
            var cliquesSizeIncidencePercentage = new float[cliquesSizeIncidence.Length];

            for(var i = 0; i < cliquesSizeIncidencePercentage.Length; i++)
            {
                cliquesSizeIncidencePercentage[i] = cliquesSizeIncidence[i] / (float)amountOfCliques * 100;
            }

            return cliquesSizeIncidencePercentage;
        }
    }
}
