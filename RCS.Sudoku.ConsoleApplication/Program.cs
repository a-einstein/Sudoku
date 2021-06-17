using RCS.Sudoku.Common;
using RCS.Sudoku.Common.Properties;
using System;
using System.Windows.Threading;

namespace RCS.Sudoku.ConsoleApplication
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            uiDispatcher = Dispatcher.CurrentDispatcher;
            sudokuHelper = new SudokuHelper(uiDispatcher);

            var taskLine = "===============================";
            Console.WriteLine(taskLine);

            string readResult;
            Cell[][] grid;

            bool fileRead = sudokuHelper.Read(out readResult, out grid);

            Console.WriteLine(readResult);

            if (fileRead)
            {
                Handle(grid);
            }
        }

        private static Dispatcher uiDispatcher;
        private static SudokuHelper sudokuHelper;

        public static void Handle(Cell[][] grid)
        {
            Show(grid);

            var timeStart = DateTime.Now;

            // HACK Disabled, see comment at CompleteFrom.
            var completed = false /*sudokuHelper.CompleteFrom(0, 0, grid)*/;
            var duration = DateTime.Now - timeStart;

            if (completed)
            {
                Console.WriteLine(string.Format (Resources.StatusSucceeded_seconds,duration));

                Show(grid);
            }
            else
                Console.WriteLine(string.Format(Resources.StatusFailed_seconds, duration));
        }

        public static void Show(Cell[][] grid)
        {
            Console.WriteLine();

            var boxLine = "+---------+---------+---------+";

            for (int row = 0; row < 9; row++)
            {
                if (row % 3 == 0) Console.WriteLine(boxLine);

                for (int column = 0; column < 9; column++)
                    Console.Write($"{(column % 3 == 0 ? "| " : " ")}{grid[row][column].ToString(true)} ");

                Console.WriteLine("|");
            }

            Console.WriteLine(boxLine);

            Console.WriteLine();
        }
    }
}
