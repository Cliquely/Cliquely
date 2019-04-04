using CliquesGraphs.CSV;
using CliquesGraphs.Excel;
using CliquesGraphs.Graphs;
using System.Collections.Generic;
using System.IO;

namespace CliquesGraphs
{
    public class CliquesGraphs
    {
        public void Plan(string[] dataFiles)
        {
            using (var excel = new ExcelGraphBook())
            {
                var dataByFiles = readDataFiles(dataFiles);

                new CliqueSizeGraph(excel).Run(dataByFiles);
                new CliquesInGenomeGraph(excel).Run(dataByFiles);
                new CliquesInArcheaGraph(excel).Run(dataByFiles);
                new CliquesInBacteriaGraph(excel).Run(dataByFiles);
                new CliquesInEukaryotaGraph(excel).Run(dataByFiles);

                excel.Save("test.xlsx");
            }
        }

        private Dictionary<string, List<CliqueRecord>> readDataFiles(string[] dataFiles)
        {
            var data = new Dictionary<string, List<CliqueRecord>>();

            foreach (var dataFile in dataFiles)
            {
                data.Add(dataFile, readCsv(dataFile));
            }

            return data;
        }

        private List<CliqueRecord> readCsv(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader))
            {
                return csv.ReadCSV();
            }
        }
    }
}
