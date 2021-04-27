using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sudoku
{
    internal class Puzzle
    {
        private static int[,] grid = new int[9, 9];

        public static bool Read()
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
                            grid[row, column] = digit;
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

        public static void Handle()
        {
            Show();

            var timeStart = DateTime.Now;
            var complete = CompleteFrom(0, 0);
            var duration = DateTime.Now - timeStart;

            if (complete)
            {
                Trace.WriteLine($"Solved in {duration}.");

                Show();
            }
            else
                Trace.WriteLine($"Failed in {duration}.");
        }

        public static void Show()
        {
            var boxLine = "+---------+---------+---------+";

            for (int row = 0; row < 9; row++)
            {
                if (row % 3 == 0) Trace.WriteLine(boxLine);

                for (int column = 0; column < 9; column++)
                    Trace.Write($"{(column % 3 == 0 ? "| " : " ")}{grid[row, column]} ");

                Trace.WriteLine("|");
            }

            Trace.WriteLine(boxLine);

            Trace.WriteLine(null);
        }

        public static bool CompleteFrom(int row, int column)
        {
            // Puzzle not completed.
            if (row < 9 && column < 9)
            {
                // Cell HAS a value.
                if (grid[row, column] != 0)
                {
                    // Row not completed.
                    if ((column + 1) < 9)
                    {
                        // Next cell in row.
                        return CompleteFrom(row, column + 1);
                    }
                    // Rows not completed.
                    else if ((row + 1) < 9)
                    {
                        // Next row.
                        return CompleteFrom(row + 1, 0);
                    }
                    // All completed from start.
                    else
                    {
                        return true;
                    }
                }
                // Cell has NO value.
                else
                {
                    // Find an acceptable digit.
                    for (int digit = 1; digit <= 9; digit++)
                    {
                        if (DigitAvailable(digit, row, column))
                        {
                            // Try digit in cell.
                            grid[row, column] = digit;

                            // Row not completed.
                            if ((column + 1) < 9)
                            {
                                // Next cell in row.
                                if (CompleteFrom(row, column + 1))
                                    // No conflicts encountered for digit in remainder of grid.
                                    return true;
                                else
                                    // Backtrack. Next digit.
                                    grid[row, column] = 0;
                            }
                            // Rows not completed.
                            else if ((row + 1) < 9)
                            {
                                // Next row.
                                if (CompleteFrom(row + 1, 0))
                                    // No conflicts encountered for digit in remainder of grid.
                                    return true;
                                else
                                    // Backtrack. Next digit.
                                    grid[row, column] = 0;
                            }
                            // No conflicts encountered for digit in remainder of grid.
                            else
                            {
                                return true;
                            }
                        }
                    }
                }

                // No completion for cell.
                return false;
            }

            // Puzzle completed.
            return true;
        }

        // TODO remove if not needed.
        struct Cell
        {
            public int Row;
            public int Column;
        }

        private static bool DigitAvailable(int digit, int row, int column)
        {
            // TODO Make this conditional.
            //Debug.WriteLine();
            //Debug.WriteLine($"Cell({cellRow},{cellColumn}), {nameof(digit)} = {digit}");

            for (int i = 0; i < 9; i++)
            {
                // Check along column at cell.
                if (grid[row, i] == digit)
                    return false;

                // Check along row at cell.
                if (grid[i, column] == digit)
                    return false;
            }

            int boxStartRow = (row / 3) * 3;
            int boxStartColumn = (column / 3) * 3;

            // Check remainder of box.
            for (int boxCellRow = boxStartRow; boxCellRow < boxStartRow + 3; boxCellRow++)
            {
                // Skip row already tested (hardly gaining).
                if (boxCellRow == row)
                    continue;

                for (int boxcellColumn = boxStartColumn; boxcellColumn < boxStartColumn + 3; boxcellColumn++)
                {
                    // Skip column already tested (hardly gaining).
                    if (boxcellColumn == column)
                        continue;

                    //Debug.WriteLine($"boxCell({boxCellRow},{boxcellColumn}) = {puzzle[boxCellRow, boxcellColumn]}");

                    if (grid[boxCellRow, boxcellColumn] == digit)
                        return false;
                }
            }

            return true;
        }
    }
}
