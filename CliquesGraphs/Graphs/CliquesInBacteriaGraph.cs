using CliquesGraphs.Excel;

namespace CliquesGraphs.Graphs
{
    class CliquesInBacteriaGraph : CliquesInGenomeGraph
    {
        public CliquesInBacteriaGraph(ExcelGraphBook excel) : base(excel, eTaxonomy.Bacteria)
        {
        }
    }
}
