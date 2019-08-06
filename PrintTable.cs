using System.Collections.Generic;
using System.Linq;
namespace nyoka_Client
{
    public class PrintTable : System.Collections.IEnumerable
    {
        public class ColumnCountException : System.Exception
        {
            public ColumnCountException()
            : base()
            {
            }
        }
        internal List<string> columnNames = new List<string>();
        internal List<int> columnWidths = new List<int>();

        internal List<List<string>> rows = new List<List<string>>();

        public PrintTable()
        {
        }
        // To enable object initializer
        public void Add(string columnName, int minWidth)
        {
            columnNames.Add(columnName);
            int maxWidth = columnName.Length > minWidth ? columnName.Length : minWidth;
            columnWidths.Add(maxWidth);
        }
        // To enable object initializer
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield break;
        }
        public void addRow(params string[] row)
        {
            if (row.Length != this.columnWidths.Count)
            {
                throw new ColumnCountException();
            }
            this.rows.Add(row.ToList());

            // expand table if necessary
            for (int i = 0; i < columnWidths.Count; i++)
            {
                if (row[i].Length > columnWidths[i])
                {
                    columnWidths[i] = row[i].Length;
                }
            }
        }
    }
}