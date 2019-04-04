using CliquesGraphs.Excel;
using System.Collections.Generic;

namespace CliquesGraphs.Graphs
{
    public abstract class GraphPlanner
    {
        private readonly ExcelGraphBook excel;
        protected readonly ExcelGraphSheet worksheet;

        public GraphPlanner(ExcelGraphBook excel)
        {
            this.excel = excel;
            worksheet = excel.AddWorkSheet(GetType().Name);
        }

        public void Run(Dictionary<string, List<CliqueRecord>> dataFiles)
        {
            runCalculations(dataFiles);
        }

        protected abstract void runCalculations(Dictionary<string, List<CliqueRecord>> dataFiles);
    }
}