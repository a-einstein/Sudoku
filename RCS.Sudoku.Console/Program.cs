using RCS.Sudoku.Common;
using System;

namespace RCS.Sudoku.ConsoleApplication
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var taskLine = "===============================";
            Console.WriteLine(taskLine);

            string readResult;
            CellContent[][] grid;

            bool fileRead = Common.Sudoku.Read(out readResult, out grid);

            Console.WriteLine(readResult);

            if (fileRead)
            {
                Handle(grid);
            }
        }

        public static void Handle(CellContent[][] grid)
        {
            Show(grid);

            var timeStart = DateTime.Now;

            // HACK Disabled, see comment at CompleteFrom.
            var completed = false /*CompleteFrom(0, 0, Grid)*/;
            var duration = DateTime.Now - timeStart;

            if (completed)
            {
                Console.WriteLine($"Completed in {duration}.");

                Show(grid);
            }
            else
                Console.WriteLine($"Failed in {duration}.");
        }

        public static void Show(CellContent[][] grid)
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
