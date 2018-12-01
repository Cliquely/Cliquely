using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BacteriaNetworks.Infrastructure;

namespace BacteriaNetworks.Tests
{
    [TestClass]
    public class BronKerboschAlgTests
    {
        private BronKerboschAlg bronKerbosch;
        private List<Gene> genes;

        [TestInitialize]
        public void init()
        {
            bronKerbosch = new BronKerboschAlg();
            genes = new List<Gene>();
        }

        [TestMethod]
        public void Should_ReturnEmpty_When_SingleGene()
        {
            var gene = new Gene(1, new List<Gene>());

            genes.Add(gene);
            Assert.IsTrue(bronKerbosch.Run(genes).Count == 0);
        }

        [TestMethod]
        public void Should_ReturnOneClique_When_GivenAGraph()
        {
            var g1 = new Gene(1, new List<Gene>());
            var g2 = new Gene(2, new List<Gene>());

            g1.Neighbors.Add(g2);
            g2.Neighbors.Add(g1);

            genes.Add(g1);
            genes.Add(g2);

            Assert.IsTrue(bronKerbosch.Run(genes).Count == 1);
        }

        [TestMethod]
        public void Should_ReturnTwoCliques_When_GivenAGraph()
        {
            var g1 = new Gene(1, new List<Gene>());
            var g2 = new Gene(2, new List<Gene>());
            var g3 = new Gene(3, new List<Gene>());

            g1.Neighbors.Add(g2);
            g2.Neighbors.Add(g1);
            g2.Neighbors.Add(g3);
            g3.Neighbors.Add(g2);

            genes.Add(g1);
            genes.Add(g2);
            genes.Add(g3);

            Assert.IsTrue(bronKerbosch.Run(genes).Count == 2);
        }

        [TestMethod]
        public void Should_ReturnTwoCliques_When_GivenAnotherGraph()
        {
            var g1 = new Gene(1, new List<Gene>());
            var g2 = new Gene(2, new List<Gene>());
            var g3 = new Gene(3, new List<Gene>());
            var g4 = new Gene(4, new List<Gene>());

            g1.Neighbors.Add(g2);
            g2.Neighbors.Add(g1);
            g2.Neighbors.Add(g3);
            g2.Neighbors.Add(g4);
            g3.Neighbors.Add(g2);
            g3.Neighbors.Add(g4);
            g4.Neighbors.Add(g2);
            g4.Neighbors.Add(g3);

            genes.Add(g1);
            genes.Add(g2);
            genes.Add(g3);
            genes.Add(g4);

            Assert.IsTrue(bronKerbosch.Run(genes).Count == 2);
        }
    }
}