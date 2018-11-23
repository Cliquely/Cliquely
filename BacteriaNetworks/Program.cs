using System.Collections.Generic;

namespace BacteriaNetworks
{
    class Program
    {
        static void Main(string[] args)
        {
            var g1 = new Gene(1, null);
            var g2 = new Gene(2, null);
            var g3 = new Gene(3, null);
            var g4 = new Gene(4, null);

            var clique1 = new List<Gene>() { g1, g2, g3 };
            var clique2 = new List<Gene>() { g1, g2, g4 };

            NetworkVisualization s = new NetworkVisualization(new List<Gene>() { g1, g2, g3, g4 }, new List<List<Gene>>() { clique1, clique2 });
            s.ShowDialog();
        }
    }
}
