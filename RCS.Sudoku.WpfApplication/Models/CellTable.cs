using RCS.Sudoku.Common.Contracts.Models;
using RCS.Sudoku.Common.Models;
using System.Data;
using System.Windows.Threading;

namespace RCS.Sudoku.WpfApplication.Models
{
    public class CellTable : DataTable, ICellGrid
    {
        public CellTable() { }

        public CellTable(Dispatcher uiDispatcher)
        {
            UiDispatcher = uiDispatcher;
        }

        public Dispatcher UiDispatcher { get; set; }

        public Cell this[int rowIndex, int columnIndex]
        {
            get => (Cell)Rows[rowIndex][columnIndex];
            set => Rows[rowIndex][columnIndex] = value;
        }

        public Cell[] this[int rowIndex]
        {
            get => (Cell[])(Rows[rowIndex].ItemArray);
            set => Rows[rowIndex].ItemArray = value;
        }

        public void Assign(Cell cell, int? digit)
        {
            // Use Dispatcher for intermediate GUI updates.
            UiDispatcher.Invoke(() =>
            {
                cell.Digit = digit;

                // Reflect changes.
                // Use of Row.SetModified was not suffcicient.
                // Actually this slows the process down considerably, which enables following it on screen without explicit stalling.
                AcceptChanges();
            }, DispatcherPriority.Send);
        }
    }
}
