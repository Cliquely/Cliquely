using CliquesGraphs.Excel;
using System.Collections.Generic;
using System.Linq;

namespace CliquesGraphs.Graphs
{
    public class CliqueSizeGraph : GraphPlanner
    {
        public CliqueSizeGraph(ExcelGraphBook excel, string worksheetName) : base(excel, worksheetName)
        {
        }

        public override void Run(Dictionary<string, List<CliqueRecord>> dataFiles, eTaxonomy taxonomy)
        {
            foreach (var (name, data) in dataFiles)
            {
                var cliquesSizes = data.Distinct().Where(clique => (clique.Taxonomy & taxonomy) > 0).Select(clique => clique.Size).ToList();
                var cliquesSizeCounter = CountCliquesSizes(cliquesSizes);
                var cliquesSizePercentage = calculatePercentages(cliquesSizeCounter, cliquesSizes.Count);

                if (worksheet["Size"] == null || worksheet["Size"]?.Items < cliquesSizeCounter.Length)
                    worksheet.AddColumn("Size", Enumerable.Range(1, cliquesSizeCounter.Length).Where(i => cliquesSizeCounter[i - 1] > 0));

                worksheet.AddColumn($"{name} count", cliquesSizeCounter.Where(i => i > 0));
                worksheet.AddColumn($"{name} percentage", cliquesSizePercentage.Where(i => i > 0));
            }
        }


        private int[] CountCliquesSizes(List<int> cliquesSizes)
        {
            var maxCliqueSize = cliquesSizes.Max();
            var cliquesSizeCounter = new int[maxCliqueSize];

            foreach (var cliqueSize in cliquesSizes) cliquesSizeCounter[cliqueSize - 1]++;

            return cliquesSizeCounter;
        }
    }
}