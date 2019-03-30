using CliquesForGenome.Configuration;
using CliquesForGenome.Network;
using CliquesForGenome.Network.Readers;
using System;
using System.Collections.Generic;

namespace CliquesForGenome
{
    class Program
    {
        static void Main(string[] args)
        {
            SettingsLoader.LoadSettings(Settings.Instance);
            writeLog($"Searching for cliques with minimum probability {Settings.Instance.MinimumProbability * 100}% and maximum clique size {Settings.Instance.MaximumCliqueSize}");

            var genomes = new GenomeLineReader();
            var genomeNetwork = new GenomeNetworks(Settings.Instance.MinimumProbability, Settings.Instance.MaximumCliqueSize);

            using (var writer = new CliquesWriter(Settings.Instance.CliquesOutputFile))
            {
                var count = 1;

                foreach (var genome in genomes.ReadAllGenomes())
                {
                    writeLog($"{count++}. Searching for {genome} cliques");
                    var cliques = genomeNetwork.SearchNetworks(genome.Abbr);
                    writeLog($"Found {cliques.Count} cliques");
                    writeLog($"Writing cliques to file");

                   writeCliques(writer, genome, cliques);
                }
            }

            Console.ReadKey();
        }

        private static void writeCliques(CliquesWriter writer, Genome genome, List<List<Gene>> cliques)
        {
            foreach (var clique in cliques)
            {
                writer.WriteClique(genome, clique);
            }

            writer.Flush();
        }

        private static void writeLog(string text)
        {
            Console.WriteLine($"[{DateTime.Now}] {text}");
        }
    }
}