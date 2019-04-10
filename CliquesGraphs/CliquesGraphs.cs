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

                new CliqueSizeGraph(excel, "Archea cliques sizes distribution").Run(dataByFiles, eTaxonomy.Archaea);
                new CliqueSizeGraph(excel, "Bacteria cliques sizes distribution").Run(dataByFiles, eTaxonomy.Bacteria);
                new CliqueSizeGraph(excel, "Eukaryota cliques sizes distribution").Run(dataByFiles, eTaxonomy.Eukaryota);
                new CliquesInGenomeGraph(excel, "Amount of cliques in Archea distribution").Run(dataByFiles, eTaxonomy.Archaea);
                new CliquesInGenomeGraph(excel, "Amount of cliques in Bacteria distribution").Run(dataByFiles, eTaxonomy.Bacteria);
                new CliquesInGenomeGraph(excel, "Amount of cliques in Eukaryota distribution").Run(dataByFiles, eTaxonomy.Eukaryota);

                excel.Save("Results.xlsx");
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
