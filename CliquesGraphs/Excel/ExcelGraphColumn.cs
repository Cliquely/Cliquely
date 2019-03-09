using System.Collections;
using System.Collections.Generic;

namespace CliquesGraphs.Excel
{
    public class ExcelGraphColumn
    {
        private readonly List<object> items;

        public ExcelGraphColumn(int index)
        {
            Index = index;
            items = new List<object>();
        }

        public int Index { get; }

        public int Count => items.Count;

        public object this[int row] => items[row - 1];

        public void AddItems(IEnumerable items)
        {
            foreach (var item in items) this.items.Add(item);
        }
    }
}