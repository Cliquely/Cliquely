using CliquesGraphs.Excel;
using System.Collections.Generic;
using System.Linq;

namespace CliquesGraphs.Graphs
{
    public class CliquesInGenomeGraph : GraphPlanner
    {
        public CliquesInGenomeGraph(ExcelGraphBook excel, string worksheetName) : base(excel, worksheetName)
        {
        }

        public override void Run(Dictionary<string, List<CliqueRecord>> dataFiles, eTaxonomy taxonomy)
        {
            foreach (var (name, data) in dataFiles)
            {
                var dataForTaxonomy = data.Where(clique => (clique.Taxonomy & taxonomy) > 0).ToList();
                var cliquesInGenome = calculateAmountOfCliquesInGenome(dataForTaxonomy);
                var cliquesSizeIncidence = calculateCliquesSizeIncidence(cliquesInGenome);
                var cliquesSizeIncidencePercentage = calculatePercentages(cliquesSizeIncidence, cliquesInGenome.Count);

                if (worksheet["Amount Of Cliques"] == null || worksheet["Amount Of Cliques"]?.Items < cliquesSizeIncidence.Length)
                    worksheet.AddColumn("Amount Of Cliques", Enumerable.Range(1, cliquesSizeIncidence.Length).Where(i => cliquesSizeIncidence[i - 1] > 0));

                worksheet.AddColumn($"{name} incidence", cliquesSizeIncidence.Where(i => i > 0));
                worksheet.AddColumn($"{name} percentage", cliquesSizeIncidencePercentage.Where(i => i > 0));
            }
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
    }
}
