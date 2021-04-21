using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Sudoku
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            int[][,] puzzles = GetPuzzles();

            foreach (var puzzle in puzzles)
            {
                Handle(puzzle);
            }
        }

        private static int[][,] GetPuzzles()
        {
            OpenPuzzle();

            int[][,] puzzles = new int[][,]
            {
                new int[,] {
                    { 3, 2, 1, 7, 0, 4, 0, 0, 0 },
                    { 6, 4, 0, 0, 9, 0, 0, 0, 7 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 4, 5, 9, 0, 0 },
                    { 0, 0, 5, 1, 8, 7, 4, 0, 0 },
                    { 0, 0, 4, 9, 6, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 2, 0, 0, 0, 7, 0, 0, 1, 9 },
                    { 0, 0, 0, 6, 0, 9, 5, 8, 2 }
                },

                // CleVR
               new int[,] {
                    {0,0,0,7,0,4,0,0,5},
                    {0,2,0,0,1,0,0,7,0},
                    {0,0,0,0,8,0,0,0,2},
                    {0,9,0,0,0,6,2,5,0},
                    {6,0,0,0,7,0,0,0,8},
                    {0,5,3,2,0,0,0,1,0},
                    {4,0,0,0,9,0,0,0,0},
                    {0,3,0,0,6,0,0,9,0},
                    {2,0,0,4,0,7,0,0,0}
                }
            };

            return puzzles;
        }

        private static void OpenPuzzle()
        {
            var initialDirectory = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\puzzles");

            var fileDialog = new OpenFileDialog
            {
                Title = "Read Sudoku",
                Filter = "TXT files|*.txt",
                InitialDirectory = initialDirectory
            };

            fileDialog.ShowDialog();
        }

        private static void Handle(int[,] puzzle)
        {
            var taskLine = "===============================";
            Debug.WriteLine(taskLine);

            Show(puzzle);

            var timeStart = DateTime.Now;
            var solved = SolveCell(puzzle, 0, 0);
            var duration = DateTime.Now - timeStart;

            if (solved)
            {
                Debug.WriteLine($"Solved in {duration}.");

                Show(puzzle);
            }
            else
                Debug.WriteLine($"Failed in {duration}.");
        }

        public static void Show(int[,] puzzle)
        {
            var boxLine = "+---------+---------+---------+";

            for (int row = 0; row < 9; row++)
            {
                if (row % 3 == 0) Debug.WriteLine(boxLine);

                for (int column = 0; column < 9; column++)
                    Debug.Write($"{(column % 3 == 0 ? "| " : " ")}{puzzle[row, column]} ");

                Debug.WriteLine("|");
            }

            Debug.WriteLine(boxLine);

            Debug.WriteLine(null);
        }

        public static bool SolveCell(int[,] puzzle, int cellRow, int cellColumn)
        {
            // Not completed.
            if (cellRow < 9 && cellColumn < 9)
            {
                // Cell HAS value.
                if (puzzle[cellRow, cellColumn] != 0)
                {
                    // Next column.
                    if ((cellColumn + 1) < 9)
                        return SolveCell(puzzle, cellRow, cellColumn + 1);

                    // Next row.
                    else if ((cellRow + 1) < 9)
                        return SolveCell(puzzle, cellRow + 1, 0);

                    else
                        return true;
                }
                // Cell has NO value.
                else
                {
                    for (int figure = 1; figure <= 9; figure++)
                    {
                        if (FigureAvailable(puzzle, cellRow, cellColumn, figure))
                        {
                            puzzle[cellRow, cellColumn] = figure;

                            if ((cellColumn + 1) < 9)
                            {
                                if (SolveCell(puzzle, cellRow, cellColumn + 1))
                                    return true;
                                else
                                    puzzle[cellRow, cellColumn] = 0;
                            }
                            else if ((cellRow + 1) < 9)
                            {
                                if (SolveCell(puzzle, cellRow + 1, 0))
                                    return true;
                                else
                                    puzzle[cellRow, cellColumn] = 0;
                            }
                            else
                                return true;
                        }
                    }
                }

                return false;
            }
            else
                return true;
        }

        // TODO remove if not needed.
        struct Cell
        {
            public int Row;
            public int Column;
        }

        private static bool FigureAvailable(int[,] puzzle, int cellRow, int cellColumn, int figure)
        {
            // TODO Make this conditional.
            //Debug.WriteLine();
            //Debug.WriteLine($"Cell({cellRow},{cellColumn}), {nameof(figure)} = {figure}");

            for (int i = 0; i < 9; i++)
            {
                // Check column at cellRow.
                if (puzzle[cellRow, i] == figure)
                    return false;

                // Check row at cellColumn.
                if (puzzle[i, cellColumn] == figure)
                    return false;
            }

            int boxStartRow = (cellRow / 3) * 3;
            int boxStartColumn = (cellColumn / 3) * 3;

            // Check box.
            // Note this also covers and its own cell and those that are already tested for the row and column.
            // TODO Should be optimized.
            for (int boxCellRow = boxStartRow; boxCellRow < boxStartRow + 3; boxCellRow++)
            {
                for (int boxcellColumn = boxStartColumn; boxcellColumn < boxStartColumn + 3; boxcellColumn++)
                {
                    //Debug.WriteLine($"boxCell({boxCellRow},{boxcellColumn}) = {puzzle[boxCellRow, boxcellColumn]}");

                    if (puzzle[boxCellRow, boxcellColumn] == figure)
                        return false;
                }
            }

            return true;
        }
    }
}
