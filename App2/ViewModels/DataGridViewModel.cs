using App2.Contracts.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Sudoku;
using System.Data;

namespace App2.ViewModels
{
    public class DataGridViewModel : ViewModelBase, INavigationAware
    {
        [PreferredConstructor]
        public DataGridViewModel()
        {
            InitTable();
        }

        public DataView Source { get; set; }

        static DataTable table;
        static DataView view;

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

            //table.Rows.Add('a', 1, 2, 3, 4, 5, 6, 7, 8, 9);
            //table.Rows.Add('b', 2, 3, 4, 5, 6, 7, 8, 9, 1);
            //table.Rows.Add('c', 3, 4, 5, 6, 7, 8, 9, 1, 2);
            //table.Rows.Add('d', 4, 5, 6, 7, 8, 9, 1, 2, 3);
            //table.Rows.Add('e', 5, 6, 7, 8, 9, 1, 2, 3, 4);
            //table.Rows.Add('f', 6, 7, 8, 9, 1, 2, 3, 4, 5);
            //table.Rows.Add('g', 7, 8, 9, 1, 2, 3, 4, 5, 6);
            //table.Rows.Add('h', 8, 9, 1, 2, 3, 4, 5, 6, 7);
            //table.Rows.Add('i', 9, 1, 2, 3, 4, 5, 6, 7, 8);

            Source = view = table.DefaultView;
        }

        public async void OnNavigatedTo(object parameter)
        {
            //Source.Clear();

            if (Puzzle.Read())
            {
                ConvertToTable(Puzzle.Grid);

                // TODO >>>
                // Generalize Puzzle to work on both DataView[][] and int[][]?
                // If at all possible.

                //Puzzle.Handle();
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
