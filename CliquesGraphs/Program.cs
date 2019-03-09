using CliquesGraphs.Excel;

namespace CliquesGraphs
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var excel = new ExcelGraphBook())
            {
                var s = new CliqueSizeGraph(excel);
                s.Run(args);
            }
        }
    }
}