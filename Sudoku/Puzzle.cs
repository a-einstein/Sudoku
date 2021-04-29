using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sudoku
{
    public class Puzzle
    {
        // Use jagged array for (supposed) speed and transferability.
        private static int[][] grid = new int[9][];

        public static int[][] Grid
        {
            get { return grid; }
            set { grid = value; }
        }

        private static DigitFrequencies digitFrequencies = new DigitFrequencies();
        private static int[] sortedDigits;

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

                    grid[row] = new int[9];

                    for (int column = 0; column < 9; column++)
                    {
                        // Currently redundant as the line should be filtered.
                        if (int.TryParse(line[column].ToString(), out int digit))
                        {
                            grid[row][column] = digit;

                            if (digit != 0)
                                digitFrequencies[digit]++;
                        }
                        else
                        {
                            Trace.WriteLine($"Error: Row {row + 1} does not have digits only.");
                            return false;
                        }
                    }
                }
            }

            sortedDigits = digitFrequencies.SortedDigits();

            return true;
        }

        public static void Handle()
        {
            Show();

            var timeStart = DateTime.Now;
            var completed = CompleteFrom(0, 0);
            var duration = DateTime.Now - timeStart;

            if (completed)
            {
                Trace.WriteLine($"Completed in {duration}.");

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
                    Trace.Write($"{(column % 3 == 0 ? "| " : " ")}{grid[row][column]} ");

                Trace.WriteLine("|");
            }

            Trace.WriteLine(boxLine);

            Trace.WriteLine(null);
        }

        public static bool CompleteFrom(int row, int column)
        {
            // Cell HAS a value.
            if (grid[row][column] != 0)
            {
                // Row not completed.
                if (++column < 9)
                {
                    // Next cell in row.
                    return CompleteFrom(row, column);
                }
                // Rows not completed.
                else if (++row < 9)
                {
                    // Start on next row.
                    return CompleteFrom(row, 0);
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

                //for (int digit = 1; digit <= 9; digit++)

                // Using initially sortedDigits instead of the normal sequence gave a significant optimization.
                // An experiment by bookkeeping the sorted fequencies only slowed down.
                // (Get a local sort, update the fequencies in local assignments, pass a copy to the next recursion.)
                foreach (var digit in sortedDigits)
                {
                    if (DigitAvailableForCell(digit, new Cell(row, column)))
                    {
                        // Try digit in cell.
                        grid[row][column] = digit;

                        // Row not completed.
                        if ((column + 1) < 9)
                        {
                            // Next cell in row.
                            if (CompleteFrom(row, column + 1))
                                // No conflicts encountered for digit in remainder of grid.
                                return true;
                            else
                            {
                                // Backtrack. Next digit.
                                grid[row][column] = 0;
                            }

                        }
                        // Rows not completed.
                        else if ((row + 1) < 9)
                        {
                            // Next row.
                            if (CompleteFrom(row + 1, 0))
                                // No conflicts encountered for digit in remainder of grid.
                                return true;
                            else
                            {
                                // Backtrack. Next digit.
                                grid[row][column] = 0;
                            }
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

        private static bool DigitAvailableForCell(int digit, Cell testCell)
        {
            // TODO Make this conditional.
            //Debug.WriteLine();
            //Debug.WriteLine($"Cell({cellRow},{cellColumn}), {nameof(digit)} = {digit}");

            for (int i = 0; i < 9; i++)
            {
                // Check along column at cell.
                if (grid[testCell.Row][i] == digit)
                    return false;

                // Check along row at cell.
                if (grid[i][testCell.Column] == digit)
                    return false;
            }

            var boxStart = new Cell(testCell.Row - (testCell.Row % 3), testCell.Column - (testCell.Column % 3));

            // Check remainder of box.
            for (int boxCellRow = boxStart.Row; boxCellRow < boxStart.Row + 3; boxCellRow++)
            {
                // Skip row already tested (hardly gaining).
                if (boxCellRow == testCell.Row)
                    continue;

                for (int boxcellColumn = boxStart.Column; boxcellColumn < boxStart.Column + 3; boxcellColumn++)
                {
                    // Skip column already tested (hardly gaining).
                    if (boxcellColumn == testCell.Column)
                        continue;

                    //Debug.WriteLine($"boxCell({boxCellRow},{boxcellColumn}) = {puzzle[boxCellRow, boxcellColumn]}");

                    if (grid[boxCellRow][boxcellColumn] == digit)
                        return false;
                }
            }

            // No conflicts for digit.
            return true;
        }
    }
}
