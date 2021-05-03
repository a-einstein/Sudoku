using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using RCS.Sudoku.Common;
using RCS.Sudoku.Gui.Contracts.ViewModels;
using System;
using System.Data;
using System.Windows.Input;

namespace RCS.Sudoku.Gui.ViewModels
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
            Source.Columns.Add(new DataColumn("a", typeof(CellContent)));
            Source.Columns.Add(new DataColumn("b", typeof(CellContent)));
            Source.Columns.Add(new DataColumn("c", typeof(CellContent)));
            Source.Columns.Add(new DataColumn("d", typeof(CellContent)));
            Source.Columns.Add(new DataColumn("e", typeof(CellContent)));
            Source.Columns.Add(new DataColumn("f", typeof(CellContent)));
            Source.Columns.Add(new DataColumn("g", typeof(CellContent)));
            Source.Columns.Add(new DataColumn("h", typeof(CellContent)));
            Source.Columns.Add(new DataColumn("i", typeof(CellContent)));
        }
        #endregion

        #region Data bindings
        public DataTable Source { get; } = new DataTable();

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

        private string solveResult = "Not tried yet";
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
            var read = Puzzle.Read(out fileResult);
            FileResult = fileResult;

            if (read)
            {
                ConvertToTable(Puzzle.Grid);
            }
        }

        void ConvertToTable(CellContent[][] grid)
        {
            for (int row = 0; row < 9; row++)
            {
                var newRow = Source.NewRow();

                // Unfortunately assigning to ItemArray does not work for int[].
                for (int column = 0; column < 9; column++)
                {
                    newRow[column] = grid[row][column];
                }

                Source.Rows.Add(newRow);
            }
        }

        private ICommand solveCommand;
        public ICommand SolveCommand => solveCommand ?? (solveCommand = new RelayCommand(Solve));

        void Solve()
        {
            var timeStart = DateTime.Now;

            // TODO Update not working anymore.
            var completed = Puzzle.CompleteFrom(0, 0, Source.Rows);
            RaisePropertyChanged(nameof(Source));

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