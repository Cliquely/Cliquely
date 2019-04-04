using CliquesGraphs.Excel;

namespace CliquesGraphs.Graphs
{
    class CliquesInEukaryotaGraph : CliquesInGenomeGraph
    {
        public CliquesInEukaryotaGraph(ExcelGraphBook excel) : base(excel, eTaxonomy.Eukaryota)
        {
        }
    }
}
