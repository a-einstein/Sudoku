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

            SolveReport(ActionStatus.Unprepared);
        }

        /// <summary>
        /// Prepare table for use and inmediate display.
        /// </summary>
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
        /// <summary>
        /// Main data structure.
        /// </summary>
        static private DataTable table = new DataTable();

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

                SolveReport(ActionStatus.Prepared);
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
        private void SolveReport(ActionStatus solveStatus, double? duration = default)
        {
            SolveStatus = solveStatus;

            // TODO Use resources.
            switch (solveStatus)
            {
                case ActionStatus.Unprepared:
                    SolveMessage = "Waiting for data.";
                    break;
                case ActionStatus.Prepared:
                    SolveMessage = "Ready to start.";
                    break;
                case ActionStatus.Started:
                    SolveMessage = "Working on it...";
                    break;
                case ActionStatus.Succeeded:
                    SolveMessage = ($"Succeeded in {duration} seconds.");
                    break;
                case ActionStatus.Failed:
                    SolveMessage = ($"Failed in {duration} seconds.");
                    break;
                default:
                    throw new Exception($"Unexpected value for {nameof(solveStatus)}.");
            }
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
            CellContent[][] grid;

            FileRead = Common.Sudoku.Read(out fileMessage, out grid);
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

        /// <summary>
        /// Command for solving sudoku.
        /// Enforce a file to be (re)read to ensure a clean slate.
        /// </summary>
        public RelayCommand SolveCommand => solveCommand ?? (solveCommand = new RelayCommand(Solve, () => SolveStatus == ActionStatus.Prepared));


        /// <summary>
        /// Solve sudoku and display results.
        /// </summary>
        void Solve()
        {
            SolveReport(ActionStatus.Started);

            var timeStart = DateTime.Now;

            var status = Common.Sudoku.CompleteFrom(0, 0, table);

            var duration = (DateTime.Now - timeStart).TotalSeconds;

            SolveReport(status, duration);
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