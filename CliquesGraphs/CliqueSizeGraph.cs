using System.Collections.Generic;
using System.IO;
using System.Linq;
using CliquesGraphs.Excel;
using CsvHelper;

namespace CliquesGraphs
{
    public class CliqueSizeGraph
    {
        private readonly ExcelGraphBook excel;

        public CliqueSizeGraph(ExcelGraphBook excel)
        {
            this.excel = excel;
        }

        public void Run(params string[] dataFiles)
        {
            var worksheet = excel.AddWorkSheet(GetType().Name);

            foreach (var dataFile in dataFiles)
            {
                var cliquesSizes = readCsv(dataFile).Select(clique => clique.Size).ToList();
                var cliquesSizeCounter = CountCliquesSizes(cliquesSizes);
                var cliquesSizePercentage = CountCliquesSizesPercentages(cliquesSizeCounter, cliquesSizes.Count);

                if (worksheet["Size"] == null || worksheet["Size"]?.Count < cliquesSizeCounter.Length)
                    worksheet.AddColumn("Size", Enumerable.Range(1, cliquesSizeCounter.Length));

                worksheet.AddColumn($"{dataFile} count", cliquesSizeCounter);
                worksheet.AddColumn($"{dataFile} percentage", cliquesSizePercentage);
            }


            worksheet.AddGraph("Cliques Sizes", "Size",
                dataFiles.Select(datafile => $"{datafile} percentage").ToArray());

            excel.Save("test.xlsx");
        }

        private List<CliqueRecord> readCsv(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader))
            {
                return csv.GetRecords<CliqueRecord>().ToList();
            }
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
                cliquesSizePercentage[i] = cliquesSizesCounter[i] / (float) numOfCliques * 100;

            return cliquesSizePercentage;
        }

        private class CliqueRecord
        {
            public int Incidence { get; set; }
            public int Size { get; set; }

            public string Proteins { get; set; }
        }
    }
}