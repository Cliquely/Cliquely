using CliquesGraphs.Excel;
using System.Collections.Generic;

namespace CliquesGraphs.Graphs
{
    public abstract class GraphPlanner
    {
        private readonly ExcelGraphBook excel;
        protected readonly ExcelGraphSheet worksheet;

        public GraphPlanner(ExcelGraphBook excel, string worksheetName)
        {
            this.excel = excel;
            worksheet = excel.AddWorkSheet(worksheetName);
        }

        public abstract void Run(Dictionary<string, List<CliqueRecord>> dataFiles, eTaxonomy taxonomy);


        protected float[] calculatePercentages(int[] cliquesData, int amountOfCliques)
        {
            var cliquesPercentage = new float[cliquesData.Length];

            for (var i = 0; i < cliquesPercentage.Length; i++)
                cliquesPercentage[i] = cliquesData[i] / (float)amountOfCliques;

            return cliquesPercentage;
        }
    }
}