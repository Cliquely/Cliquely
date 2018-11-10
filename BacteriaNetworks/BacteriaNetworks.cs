using System;
using System.Collections.Generic;

namespace BacteriaNetworks
{
    public class BacteriaNetworks
    {
        public List<List<Gene>> searchNetworks(string bacteria, float minimumProbability)
        {
            var genesOfBacteria = getGenesOfBacteria(bacteria, minimumProbability);

            return new BronKerboschAlg().Run(genesOfBacteria);
        }

        private List<Gene> getGenesOfBacteria(string bacteria, float minimumProbability)
        {
            throw new NotImplementedException();
        }
    }
}
