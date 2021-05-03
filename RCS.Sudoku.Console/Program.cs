using RCS.Sudoku.Common;
using System;
using System.Diagnostics;

namespace RCS.Sudoku.Console
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Output currently only works in debug mode.
            // TODO Transform to Console again.
            Debug.WriteLine("Test Debug");
            Trace.WriteLine("Test Trace");

            var taskLine = "===============================";
            Trace.WriteLine(taskLine);

            string result;
            CellContent[][] grid;

            if (Common.Sudoku.Read(out result, out grid))
            {
                Handle(grid);
            }
        }

        public static void Handle(CellContent[][] grid)
        {
            Show(grid);

            var timeStart = DateTime.Now;
            // HACK See comment at CompleteFrom.
            var completed = false /*CompleteFrom(0, 0, Grid)*/;
            var duration = DateTime.Now - timeStart;

            if (completed)
            {
                Trace.WriteLine($"Completed in {duration}.");

                Show(grid);
            }
            else
                Trace.WriteLine($"Failed in {duration}.");
        }

        public static void Show(CellContent[][] grid)
        {
            var boxLine = "+---------+---------+---------+";

            for (int row = 0; row < 9; row++)
            {
                if (row % 3 == 0) Trace.WriteLine(boxLine);

                for (int column = 0; column < 9; column++)
                    Trace.Write($"{(column % 3 == 0 ? "| " : " ")}{grid[row][column].ToString(true)} ");

                Trace.WriteLine("|");
            }

            Trace.WriteLine(boxLine);

            Trace.WriteLine(null);
        }
    }
}
