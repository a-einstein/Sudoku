using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sudoku
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // TODO Output only works in debug mode.
            // Either get working as Trace/Debug, to window or file (see WpfShop too), or create a GUI.
            Debug.WriteLine("Test Debug");
            Trace.WriteLine("Test Trace");

            var taskLine = "===============================";
            Trace.WriteLine(taskLine);

            if (ReadPuzzle())
            {
                Handle();
            }
        }

        static int[,] puzzle = new int[9, 9];

        private static bool ReadPuzzle()
        {
            var initialDirectory = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\..\\puzzles");

            var fileDialog = new OpenFileDialog
            {
                Title = "Read Sudoku",
                Filter = "TXT files|*.txt",
                InitialDirectory = initialDirectory
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var filename = Path.GetFileName(fileDialog.FileName);
                Trace.WriteLine($"File = '{filename}'.");

                string[] fileLines = File.ReadAllLines(fileDialog.FileName);

                if (fileLines.Length != 9)
                {
                    Trace.WriteLine($"Error: Puzzle does not have 9 rows.");
                    return false;
                }

                for (int row = 0; row < 9; row++)
                {
                    var fileLine = fileLines[row];

                    // Only keep digits.
                    var line = Regex.Replace(fileLine, @"\D", "");

                    if (line.Length != 9)
                    {
                        Trace.WriteLine($"Error: Row {row + 1} does not have 9 digits.");
                        return false;
                    }

                    for (int column = 0; column < 9; column++)
                    {
                        // Currently redundant as the line should be filtered.
                        if (int.TryParse(line[column].ToString(), out int digit))
                        {
                            puzzle[row, column] = digit;
                        }
                        else
                        {
                            Trace.WriteLine($"Error: Row {row + 1} does not have digits only.");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static void Handle()
        {
            Show(puzzle);

            var timeStart = DateTime.Now;
            var solved = SolveCell(0, 0);
            var duration = DateTime.Now - timeStart;

            if (solved)
            {
                Trace.WriteLine($"Solved in {duration}.");

                Show(puzzle);
            }
            else
                Trace.WriteLine($"Failed in {duration}.");
        }

        public static void Show(int[,] puzzle)
        {
            var boxLine = "+---------+---------+---------+";

            for (int row = 0; row < 9; row++)
            {
                if (row % 3 == 0) Trace.WriteLine(boxLine);

                for (int column = 0; column < 9; column++)
                    Trace.Write($"{(column % 3 == 0 ? "| " : " ")}{puzzle[row, column]} ");

                Trace.WriteLine("|");
            }

            Trace.WriteLine(boxLine);

            Trace.WriteLine(null);
        }

        public static bool SolveCell(int cellRow, int cellColumn)
        {
            // Puzzle not completed.
            if (cellRow < 9 && cellColumn < 9)
            {
                // Cell HAS value.
                if (puzzle[cellRow, cellColumn] != 0)
                {
                    if ((cellColumn + 1) < 9)
                        // Next column.
                        return SolveCell(cellRow, cellColumn + 1);

                    else if ((cellRow + 1) < 9)
                        // Next row.
                        return SolveCell(cellRow + 1, 0);

                    else
                        return true;
                }
                // Cell has NO value.
                else
                {
                    for (int digit = 1; digit <= 9; digit++)
                    {
                        if (DigitAvailable(cellRow, cellColumn, digit))
                        {
                            puzzle[cellRow, cellColumn] = digit;

                            if ((cellColumn + 1) < 9)
                            {
                                // Next column.
                                if (SolveCell(cellRow, cellColumn + 1))
                                    return true;
                                else
                                    // Next row.
                                    puzzle[cellRow, cellColumn] = 0;
                            }
                            else if ((cellRow + 1) < 9)
                            {
                                if (SolveCell(cellRow + 1, 0))
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

        private static bool DigitAvailable(int cellRow, int cellColumn, int digit)
        {
            // TODO Make this conditional.
            //Debug.WriteLine();
            //Debug.WriteLine($"Cell({cellRow},{cellColumn}), {nameof(digit)} = {digit}");

            for (int i = 0; i < 9; i++)
            {
                // Check column at cellRow.
                if (puzzle[cellRow, i] == digit)
                    return false;

                // Check row at cellColumn.
                if (puzzle[i, cellColumn] == digit)
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

                    if (puzzle[boxCellRow, boxcellColumn] == digit)
                        return false;
                }
            }

            return true;
        }
    }
}
