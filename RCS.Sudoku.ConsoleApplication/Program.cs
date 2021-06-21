using RCS.Sudoku.Common.Models;
using RCS.Sudoku.Common.Properties;
using RCS.Sudoku.Common.Services;
using RCS.Sudoku.ConsoleApplication.Models;
using System;

namespace RCS.Sudoku.ConsoleApplication
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ConsoleKeyInfo keyInfo;

            do
            {
                var taskLine = "=====================================";

                Console.WriteLine();
                Console.WriteLine(taskLine);

                string readResult;
                Cell[][] fileGrid;

                bool fileRead = sudokuService.Read(out readResult, out fileGrid);

                CellGrid grid = new CellGrid(fileGrid);

                Console.WriteLine();
                Console.WriteLine(readResult);

                if (fileRead)
                {
                    Handle(grid);
                }

                Console.WriteLine();
                Console.WriteLine(Resources.RepeatOrQuit);

                keyInfo = Console.ReadKey();

            } while (keyInfo.Key != ConsoleKey.Escape);

            // Note the console will only directly close if not running from Visual Studio.
        }

        private static SudokuService sudokuService = new SudokuService();

        /// <summary>
        /// Attempt to solve a sudoku and display result.
        /// </summary>
        /// <param name="grid">Sudoku puzzle.</param>
        public static void Handle(CellGrid grid)
        {
            // TODO Share more with SudokuViewModel.

            Show(grid);

            sudokuService.Grid = grid;

            var timeStart = DateTime.Now;

            var status = sudokuService.CompleteFrom(0, 0);

            var duration = (DateTime.Now - timeStart).TotalSeconds;

            Console.WriteLine();

            if (status == ActionStatus.Succeeded)
            {
                Console.WriteLine(string.Format(Resources.StatusSucceeded_seconds, duration));

                Show(grid);
            }
            else
                Console.WriteLine(string.Format(Resources.StatusFailed_seconds, duration));
        }

        /// <summary>
        ///  Write the current state of the grid on screen.
        /// </summary>
        /// <param name="grid">Sudoku puzzle.</param>
        public static void Show(CellGrid grid)
        {
            Console.WriteLine();

            var boxLine = "+---------+---------+---------+";

            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                if (rowIndex % 3 == 0) Console.WriteLine(boxLine);

                for (int columnIndex = 0; columnIndex < 9; columnIndex++)
                    // Note dot ASCII 250 is used, not a period.
                    Console.Write($"{(columnIndex % 3 == 0 ? "| " : " ")}{grid[rowIndex][columnIndex].ToString("·")} ");

                Console.WriteLine("|");
            }

            Console.WriteLine(boxLine);
        }
    }
}
