using CliquesGraphs.Excel;

namespace CliquesGraphs.Graphs
{
    public class CliquesInArcheaGraph : CliquesInGenomeGraph
    {
        public CliquesInArcheaGraph(ExcelGraphBook excel) : base(excel, eTaxonomy.Archaea)
        {
        }
    }
}
