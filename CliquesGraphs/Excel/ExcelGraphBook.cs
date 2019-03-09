using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace CliquesGraphs.Excel
{
    public class ExcelGraphBook : IDisposable
    {
        private readonly Dictionary<string, ExcelGraphSheet> excelSheets;
        private readonly ExcelPackage package;

        public ExcelGraphBook()
        {
            package = new ExcelPackage();
            excelSheets = new Dictionary<string, ExcelGraphSheet>();
        }

        public void Dispose()
        {
            package.Dispose();
        }

        public ExcelGraphSheet AddWorkSheet(string name)
        {
            var excelSheet = new ExcelGraphSheet(package.Workbook.Worksheets.Add(name));

            excelSheets.Add(name, excelSheet);

            return excelSheet;
        }

        public void Save(string name)
        {
            foreach (var excelGraphSheet in excelSheets.Values) excelGraphSheet.Save();

            package.SaveAs(new FileInfo(name));
        }
    }
}