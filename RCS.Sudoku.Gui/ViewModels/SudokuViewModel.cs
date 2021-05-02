using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using RCS.Sudoku.Common;
using RCS.Sudoku.Console;
using RCS.Sudoku.Gui.Contracts.ViewModels;
using System.Data;

namespace RCS.Sudoku.Gui.ViewModels
{
    public class SudokuViewModel : ViewModelBase, INavigationAware
    {
        [PreferredConstructor]
        public SudokuViewModel()
        {
            InitTable();
        }

        public DataTable Source { get; set; }
        static DataTable table;

        private void InitTable()
        {
            table = new DataTable();

            // Impovised numbering of rows. Needs name.
            // Note this changes indexing!
            //table.Columns.Add(new DataColumn("rij", typeof(char)));

            table.Columns.Add(new DataColumn("a", typeof(CellContent)));
            table.Columns.Add(new DataColumn("b", typeof(CellContent)));
            table.Columns.Add(new DataColumn("c", typeof(CellContent)));
            table.Columns.Add(new DataColumn("d", typeof(CellContent)));
            table.Columns.Add(new DataColumn("e", typeof(CellContent)));
            table.Columns.Add(new DataColumn("f", typeof(CellContent)));
            table.Columns.Add(new DataColumn("g", typeof(CellContent)));
            table.Columns.Add(new DataColumn("h", typeof(CellContent)));
            table.Columns.Add(new DataColumn("i", typeof(CellContent)));

            Source = table;
        }

        public async void OnNavigatedTo(object parameter)
        {
            if (Puzzle.Read())
            {
                ConvertToTable(Puzzle.Grid);

                Puzzle.CompleteFrom(0, 0, table.Rows);
            }
        }

        private static void ConvertToTable(CellContent[][] grid)
        {
            for (int row = 0; row < 9; row++)
            {
                var newRow = table.NewRow();

                // Unfortunately assigning to ItemArray does not work for int[].
                for (int column = 0; column < 9; column++)
                {
                    newRow[column] = grid[row][column];
                }

                table.Rows.Add(newRow);
            }
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
