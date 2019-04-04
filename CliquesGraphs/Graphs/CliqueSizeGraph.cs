using CliquesGraphs.Excel;
using System.Collections.Generic;
using System.Linq;

namespace CliquesGraphs.Graphs
{
    public class CliqueSizeGraph : GraphPlanner
    {
        public CliqueSizeGraph(ExcelGraphBook excel) : base(excel)
        {
        }

        protected override void runCalculations(Dictionary<string, List<CliqueRecord>> dataFiles)
        {
            foreach (var (name, data) in dataFiles)
            {
                var cliquesSizes = data.Distinct().Select(clique => clique.Size).ToList();
                var cliquesSizeCounter = CountCliquesSizes(cliquesSizes);
                var cliquesSizePercentage = CountCliquesSizesPercentages(cliquesSizeCounter, cliquesSizes.Count);

                if (worksheet["Size"] == null || worksheet["Size"]?.Items < cliquesSizeCounter.Length)
                    worksheet.AddColumn("Size", Enumerable.Range(1, cliquesSizeCounter.Length).Where(i => cliquesSizeCounter[i - 1] > 0));

                worksheet.AddColumn($"{name} count", cliquesSizeCounter.Where(i => i > 0));
                worksheet.AddColumn($"{name} percentage", cliquesSizePercentage.Where(i => i > 0));
            }


            worksheet.AddGraph("Cliques Sizes", "Size",
                dataFiles.Select(datafile => $"{datafile.Key} percentage").ToArray());

        }


        private int[] CountCliquesSizes(List<int> cliquesSizes)
        {
            var maxCliqueSize = cliquesSizes.Max();
            var cliquesSizeCounter = new int[maxCliqueSize];

            foreach (var cliqueSize in cliquesSizes) cliquesSizeCounter[cliqueSize - 1]++;

            return cliquesSizeCounter;
        }

        private float[] CountCliquesSizesPercentages(int[] cliquesSizesCounter, int numOfCliques)
        {
            var cliquesSizePercentage = new float[cliquesSizesCounter.Length];

            for (var i = 0; i < cliquesSizePercentage.Length; i++)
                cliquesSizePercentage[i] = cliquesSizesCounter[i] / (float)numOfCliques * 100;

            return cliquesSizePercentage;
        }
    }
}