using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using RCS.Sudoku.Common;
using RCS.Sudoku.WpfApplication.Contracts.ViewModels;
using System;
using System.Data;
using System.Windows.Input;

namespace RCS.Sudoku.WpfApplication.ViewModels
{
    public class SudokuViewModel : ViewModelBase, INavigationAware
    {
        #region Construction
        [PreferredConstructor]
        public SudokuViewModel()
        {
            InitTable();
        }

        private void InitTable()
        {
            // Define table.
            table.Columns.Add(new DataColumn("a", typeof(CellContent)));
            table.Columns.Add(new DataColumn("b", typeof(CellContent)));
            table.Columns.Add(new DataColumn("c", typeof(CellContent)));
            table.Columns.Add(new DataColumn("d", typeof(CellContent)));
            table.Columns.Add(new DataColumn("e", typeof(CellContent)));
            table.Columns.Add(new DataColumn("f", typeof(CellContent)));
            table.Columns.Add(new DataColumn("g", typeof(CellContent)));
            table.Columns.Add(new DataColumn("h", typeof(CellContent)));
            table.Columns.Add(new DataColumn("i", typeof(CellContent)));

            // Add empty rows for visual appeal.
            for (int row = 0; row < 9; row++)
            {
                var emptyRow = table.NewRow();

                for (int column = 0; column < 9; column++)
                {
                    // Initialize for proper binding.
                    emptyRow[column] = new CellContent(0);
                }

                table.Rows.Add(emptyRow);
            }
        }
        #endregion

        #region Data
        static private DataTable table = new DataTable();

        public DataView Source { get; } = table.DefaultView;


        private bool fileRead;
        private bool FileRead
        {
            get { return fileRead; }
            set
            {
                fileRead = value;

                SolveResult = solveResultDefault;
                solveCommand.RaiseCanExecuteChanged();
            }
        }

        private string fileResult = "No file yet";
        public string FileResult
        {
            get { return fileResult; }
            set
            {
                // Using this simple way instead of Set, which did not work.
                fileResult = value;
                RaisePropertyChanged(nameof(FileResult));
            }
        }

        private const string solveResultDefault = "Not tried yet";
        private string solveResult = solveResultDefault;
        public string SolveResult
        {
            get { return solveResult; }
            set
            {
                // Using this simple way instead of Set, which did not work.
                solveResult = value;
                RaisePropertyChanged(nameof(SolveResult));
            }
        }
        #endregion

        #region Commands
        private ICommand readFileCommand;
        public ICommand ReadFileCommand => readFileCommand ?? (readFileCommand = new RelayCommand(ReadFile));

        private void ReadFile()
        {
            CellContent[][] grid;

            FileRead = Common.Sudoku.Read(out fileResult, out grid);
            FileResult = fileResult;

            if (FileRead)
            {
                FillTable(grid);
            }
        }

        void FillTable(CellContent[][] grid)
        {
            for (int row = 0; row < 9; row++)
            {
                // Unfortunately assigning to ItemArray does not work for int[].
                for (int column = 0; column < 9; column++)
                {
                    table.Rows[row][column] = grid[row][column];
                }
            }

            // Note this generally is needed to update the view.
            table.AcceptChanges();
        }

        private RelayCommand solveCommand;
        public ICommand SolveCommand => solveCommand ?? (solveCommand = new RelayCommand(Solve, (() => FileRead)));

        void Solve()
        {
            SolveResult = "Working on it...";

            var timeStart = DateTime.Now;

            var completed = Common.Sudoku.CompleteFrom(0, 0, table);

            var duration = (DateTime.Now - timeStart).TotalSeconds;

            if (completed)
            {
                SolveResult = ($"Completed in {duration} seconds.");
            }
            else
            {
                SolveResult = ($"Failed in {duration} seconds.");
            }
        }
        #endregion

        #region Navigation
        public async void OnNavigatedTo(object parameter)
        { }

        public void OnNavigatedFrom()
        { }
        #endregion
    }
}