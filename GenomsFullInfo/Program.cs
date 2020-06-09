using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GenomsFullInfo
{
    class Program
    {
        public static async Task Main()
        {
            var genomes = await GetGenomes(@"data\genome_line.txt");
            await GetGenomesNumberOfGenes(genomes, @"data\cleaned_data_reverse.txt");
            await GetGenomesNumberOfCliques(genomes, @"data\Cliques100%.csv");
            await GetGenomesNumberOfClusters(genomes, @"C:\Users\NoamChapnik\Desktop\mbgd_2016-01_extended.tab");

            await WriteToCsv(genomes, "Genomes Details.csv");
        }

        private static async Task WriteToCsv(Dictionary<string, Genome> genomes, string filename)
        {
            using var writer = new StreamWriter(filename);

            await writer.WriteLineAsync("Abbreviation, FullName, Taxonomy, NumberOfGenes, NumberOfClusters, NumberOfCliques");

            foreach(var genome in genomes.Values)
            {
                await writer.WriteLineAsync($"{genome.Abbreviation}, {genome.FullName}, {genome.Taxonomy}, {genome.NumberOfGenes}, {genome.NumberOfClusters}, {genome.NumberOfCliques}");
            }
        }

        private static async Task<Dictionary<string, Genome>> GetGenomes(string filename)
        {
            var genomes = new Dictionary<string, Genome>(4742);
            var genomesReader = DataFilesReader.ReadFile(filename);

            //taxnomy   Abbreviation fullname
            await foreach (var genome in genomesReader)
            {
                genomes.Add(genome[1], new Genome { Taxonomy = genome[0], Abbreviation = genome[1], FullName = genome[2] });
            }

            return genomes;
        }

        private static async Task GetGenomesNumberOfClusters(Dictionary<string, Genome> genomes, string reversedDataFileName)
        {
            var reverseDataReader = DataFilesReader.ReadFile(reversedDataFileName, 2);

            //Abbreviation  count   list
            await foreach(var genome in reverseDataReader)
            {
                var abbreviation = genome[0];

                if (genomes.ContainsKey(abbreviation))
                {
                    genomes[abbreviation].NumberOfClusters = int.Parse(genome[1]);
                }
            }
        }

        private static async Task GetGenomesNumberOfCliques(Dictionary<string, Genome> genomes, string cliquesFileName)
        {
            var cliquesReader = DataFilesReader.ReadFile(cliquesFileName, 1, ',');

            //Taxonomy  fullName    Abbreviation    AmountOfGenes   GenesList
            await foreach (var clique in cliquesReader)
            {
                var abbreviation = clique[2];

                if (genomes.ContainsKey(abbreviation))
                {
                    genomes[abbreviation].NumberOfCliques++;
                }
            }
        }

        private static async Task GetGenomesNumberOfGenes(Dictionary<string, Genome> genomes, string clustersFileName)
        {
            var clustersReader = DataFilesReader.ReadFile(clustersFileName, 9, splitOptions: StringSplitOptions.None);

            await foreach (var row in clustersReader)
            {
                var genes = row[8..];

                foreach (var gene in genes)
                {
                    if (!string.IsNullOrEmpty(gene))
                    {
                        var abbreviation = gene.Substring(0, gene.IndexOf(':'));
                        var amount = gene.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

                        if (genomes.ContainsKey(abbreviation))
                        {
                            genomes[abbreviation].NumberOfGenes += amount;
                        }
                    }
                }
            }
        }
    }
}
