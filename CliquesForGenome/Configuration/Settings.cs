using Microsoft.Extensions.Configuration;
using System;

namespace CliquesForGenome.Configuration
{
    public class Settings
    {
        private static readonly Lazy<Settings> lazyLoader = new Lazy<Settings>(() => new Settings());

        public static Settings Instance => lazyLoader.Value;

        private Settings()
        {
            
        }

        public string GenomesFile { get; set; }
        public string GenomesForGenesFile { get; set; }
        public string GenesForGenomesFile { get; set; }
        public string CliquesOutputFile { get; set; }

        public float MinimumProbability { get; set; }
        public int MaximumCliqueSize { get; set; }
    }
}
