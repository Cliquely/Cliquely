using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CliquesForGenome
{
    public class CliquesWriter : IDisposable
    {
        private readonly StreamWriter cliquesWriter;

        public CliquesWriter(string cliquesFileName, bool append = false)
        {
            cliquesWriter = new StreamWriter(cliquesFileName, append);
            
            if(!append)
            {
                writeHeaders();
            }
        }

        private void writeHeaders()
        {
            cliquesWriter.WriteLine("Taxonomy,Organism,Abbrev,Size,Genes");
        }

        public void WriteClique(Genome genome, List<Gene> clique)
        {
            cliquesWriter.WriteLine($"{genome.Taxonomy},{genome.Name},{genome.Abbr},{clique.Count},{getCliqueGenesforCSV(clique)}");
        }

        private string getCliqueGenesforCSV(List<Gene> clique)
        {
            return clique.Select(gene => gene.Id.ToString()).Aggregate((gene1, gene2) => $"{gene1},{gene2}");
        }

        public void Flush()
        {
            cliquesWriter.Flush();
        }

        public void Dispose()
        {
            cliquesWriter.Dispose();
        }
    }
}