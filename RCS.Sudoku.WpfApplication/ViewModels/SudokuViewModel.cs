using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using RCS.Sudoku.Common.Models;
using RCS.Sudoku.Common.Properties;
using RCS.Sudoku.Common.Services;
using RCS.Sudoku.WpfApplication.Contracts.ViewModels;
using RCS.Sudoku.WpfApplication.Models;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

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

        private Dispatcher uiDispatcher = Dispatcher.CurrentDispatcher;
        private SudokuService sudokuService = new SudokuService();

        /// <summary>
        /// Prepare table for use and inmediate display.
        /// </summary>
        private void InitTable()
        {
            table.UiDispatcher = uiDispatcher;

            // Define table.
            table.Columns.Add(new DataColumn("a", typeof(Cell)));
            table.Columns.Add(new DataColumn("b", typeof(Cell)));
            table.Columns.Add(new DataColumn("c", typeof(Cell)));
            table.Columns.Add(new DataColumn("d", typeof(Cell)));
            table.Columns.Add(new DataColumn("e", typeof(Cell)));
            table.Columns.Add(new DataColumn("f", typeof(Cell)));
            table.Columns.Add(new DataColumn("g", typeof(Cell)));
            table.Columns.Add(new DataColumn("h", typeof(Cell)));
            table.Columns.Add(new DataColumn("i", typeof(Cell)));

            var emptyCells = new Cell[9] { new Cell(), new Cell(), new Cell(), new Cell(), new Cell(), new Cell(), new Cell(), new Cell(), new Cell() };

            // Add empty rows for visual appeal.
            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                // Note NewRowArray() is protected.
                table.Rows.Add(table.NewRow());
                table[rowIndex] = emptyCells;
            }

            table.AcceptChanges();
        }
        #endregion

        #region Data
        /// <summary>
        /// Main data structure.
        /// </summary>
        static private CellTable table = new CellTable();

        /// <summary>
        /// Bindable view.
        /// </summary>
        public DataView Source { get; } = table.DefaultView;

        private bool fileRead;

        /// <summary>
        /// Status of file been read. 
        /// Implies enablement and status of solving.
        /// </summary>
        private bool FileRead
        {
            get { return fileRead; }
            set
            {
                fileRead = value;

                Report(ActionStatus.Prepared);
            }
        }

        private string fileMessage = "No file yet";

        /// <summary>
        /// Verbal status of file been read.
        /// </summary>
        public string FileMessage
        {
            get { return fileMessage; }
            set
            {
                // Using this simple way instead of Set, which did not work.
                fileMessage = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Set SolveStatus and SolveMessage in conjunction, with a duration if applicable.
        /// </summary>
        /// <param name="solveStatus">New status.</param>
        /// <param name="duration">Used time.</param>
        private void Report(ActionStatus solveStatus, double? duration = default)
        {
            SolveStatus = solveStatus;

            SolveMessage = solveStatus switch
            {
                ActionStatus.Unprepared => Resources.StatusUnprepared,
                ActionStatus.Prepared => Resources.StatusPrepared,
                ActionStatus.Started => Resources.StatusStarted,
                ActionStatus.Succeeded => string.Format(Resources.StatusSucceeded_seconds, duration),
                ActionStatus.Failed => string.Format(Resources.StatusFailed_seconds, duration),
                _ => throw new Exception($"Unexpected value for {nameof(solveStatus)}.")
            };
        }

        private ActionStatus solveStatus = ActionStatus.Unprepared;

        /// <summary>
        /// Status of sudoku been solved.
        /// </summary>
        private ActionStatus SolveStatus
        {
            get { return solveStatus; }
            set
            {
                solveStatus = value;
                SolveCommand.RaiseCanExecuteChanged();
            }
        }

        private string solveMessage;

        /// <summary>
        /// Verbal status of sudoku been solved.
        /// </summary>
        public string SolveMessage
        {
            get { return solveMessage; }
            set
            {
                // Using this simple way instead of Set, which did not work.
                solveMessage = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        private ICommand readFileCommand;
        public ICommand ReadFileCommand => readFileCommand ?? (readFileCommand = new RelayCommand(ReadFile));

        /// <summary>
        /// Read and convert data.
        /// </summary>
        private void ReadFile()
        {
            Cell[][] grid;

            FileRead = sudokuService.Read(out fileMessage, out grid);
            FileMessage = fileMessage;

            if (FileRead)
            {
                FillTable(grid);
            }
        }

        /// <summary>
        /// Convert data.
        /// </summary>
        /// <param name="grid"></param>
        void FillTable(Cell[][] grid)
        {
            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                table[rowIndex] = grid[rowIndex];
            }

            // Note this generally is needed to update the view.
            table.AcceptChanges();
        }

        private RelayCommand solveCommand;

        /// <summary>
        /// Command for solving sudoku.
        /// Enforce a file to be (re)read to ensure a clean slate.
        /// </summary>
        public RelayCommand SolveCommand => solveCommand ?? (solveCommand = new RelayCommand(Solve, () => SolveStatus == ActionStatus.Prepared));

        /// <summary>
        /// Attempt to solve a sudoku and display result.
        /// </summary>
        private void Solve()
        {
            // Use Run on a non UI-thread and Dispatcher to enable intermediate updates back on the UI-thread.
            Task.Run(() =>
            {
                uiDispatcher.Invoke(() => Report(ActionStatus.Started), DispatcherPriority.Send);

                double duration;

                var status = sudokuService.Solve(table, out duration);

                uiDispatcher.Invoke(() => Report(status, duration), DispatcherPriority.Send);
            });
        }
        #endregion

        #region Navigation
        public async void OnNavigatedTo(object parameter)
        {
            Report(ActionStatus.Unprepared);
        }

        public void OnNavigatedFrom()
        { }
        #endregion
    }
}