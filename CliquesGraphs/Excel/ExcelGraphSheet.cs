using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace CliquesGraphs.Excel
{
    public class ExcelGraphSheet
    {
        private readonly Dictionary<string, ExcelGraphColumn> columns;
        private readonly ExcelWorksheet workSheet;

        public ExcelGraphSheet(ExcelWorksheet workSheet)
        {
            this.workSheet = workSheet;
            columns = new Dictionary<string, ExcelGraphColumn>();
        }

        public ExcelGraphColumn this[string header] => columns.ContainsKey(header) ? columns[header] : null;

        public void AddColumn(string header, IEnumerable column)
        {
            var columnIndex = columns.Count + 1;

            if (columns.ContainsKey(header))
                columnIndex = columns[header].Index;

            columns[header] = new ExcelGraphColumn(columnIndex);
            columns[header].AddItems(column);
        }

        public void AddGraph(string name, string xSeriesColumn, params string[] seriesColumns)
        {
            var chart = workSheet.Drawings.AddChart(name, eChartType.Line);
            chart.SetPosition(5, 0, columns.Count + 1, 0);
            chart.SetSize(700, 400);

            var toRow = getMaximumRow();
            var xSeriesAddress =
                ExcelCellBase.GetAddress(2, columns[xSeriesColumn].Index, toRow, columns[xSeriesColumn].Index);

            foreach (var column in seriesColumns)
            {
                var series =
                    chart.Series.Add(ExcelCellBase.GetAddress(2, columns[column].Index, toRow, columns[column].Index),
                        xSeriesAddress);
                series.Header = column;
            }
        }

        private int getMaximumRow()
        {
            return columns.Values.Max(col => col.Items) + 1;
        }

        public void Save()
        {
            foreach (var (header, col) in columns)
            {
                workSheet.Cells[1, col.Index].Value = header;

                for (var row = 2; row <= col.Items + 1; row++) workSheet.Cells[row, col.Index].Value = col[row - 1];
            }
        }
    }
}