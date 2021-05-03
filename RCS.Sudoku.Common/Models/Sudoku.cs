using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RCS.Sudoku.Common
{
    public class Sudoku
    {
        private static DigitFrequencies digitFrequencies = new DigitFrequencies();
        private static int[] sortedDigits;

        public static bool Read(out string result, out CellContent[][] grid)
        {
            // Use jagged array for (supposed) speed and transferability.
            grid = new CellContent[9][];

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
                    result = $"Error: Puzzle does not have 9 rows.";
                    Trace.WriteLine(result);
                    return false;
                }

                for (int row = 0; row < 9; row++)
                {
                    var fileLine = fileLines[row];

                    // Only keep digits.
                    var line = Regex.Replace(fileLine, @"\D", "");

                    if (line.Length != 9)
                    {
                        result = $"Error: Row {row + 1} does not have 9 digits.";
                        Trace.WriteLine(result);
                        return false;
                    }

                    grid[row] = new CellContent[9];

                    for (int column = 0; column < 9; column++)
                    {
                        // Currently redundant as the line should be filtered.
                        if (int.TryParse(line[column].ToString(), out int digit))
                        {
                            grid[row][column] = new CellContent(digit);

                            if (digit != 0)
                                digitFrequencies[digit]++;
                        }
                        else
                        {
                            result = $"Error: Row {row + 1} does not have digits only.";
                            Trace.WriteLine(result);
                            return false;
                        }
                    }
                }

                sortedDigits = digitFrequencies.SortedDigits();

                result = filename;
                return true;
            }
            else
            {
                result = "No file read.";
                return false;
            }
        }

        // Currently gave up on the idea to make this generic for both a direct grid and a DataTable/DataView on CellContent.
        // Problem is that DataTable and DataView don't implement IList on both the rows and columns.
        // Chose for this option with a table to enable easy binding to the view.
        public static bool CompleteFrom(int row, int column, DataTable table)
        {
            var cellContent = (CellContent)table.Rows[row][column];

            // Cell HAS a value.
            if (cellContent.Digit.HasValue)
            {
                // Row not completed.
                if (++column < 9)
                {
                    // Next cell in row.
                    return CompleteFrom(row, column, table);
                }
                // Rows not completed.
                else if (++row < 9)
                {
                    // Start on next row.
                    return CompleteFrom(row, 0, table);
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
                // Using initially sortedDigits instead of the normal sequence gave a significant optimization.
                // An experiment by bookkeeping the sorted fequencies only slowed down.
                // (Get a local sort, update the fequencies in local assignments, pass a copy to the next recursion.)
                foreach (var digit in sortedDigits)
                {
                    if (DigitAvailableForCell(digit, new CellLocation(row, column), table))
                    {
                        // Try digit in cell.
                        Assign(cellContent, digit, table);

                        // Row not completed.
                        if ((column + 1) < 9)
                        {
                            // Next cell in row.
                            if (CompleteFrom(row, column + 1, table))
                                // No conflicts encountered for digit in remainder of table.
                                return true;
                            else
                            {
                                // Backtrack. Next digit.
                                Assign(cellContent, null, table);
                            }

                        }
                        // Rows not completed.
                        else if ((row + 1) < 9)
                        {
                            // Next row.
                            if (CompleteFrom(row + 1, 0, table))
                                // No conflicts encountered for digit in remainder of table.
                                return true;
                            else
                            {
                                // Backtrack. Next digit.
                                Assign(cellContent, null, table);
                            }
                        }
                        // No conflicts encountered for digit in remainder of table.
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

        // This could be part of CellContent, but I kept DataTable out of there.
        private static void Assign(CellContent cellContent, int? digit, DataTable table)
        {
            cellContent.Digit = digit;

            // Reflect changes.
            // TODO Looking for a working way to delay, while updating view.
            table.AcceptChanges();
        }

        private static bool DigitAvailableForCell(int digit, CellLocation testCell, DataTable table)
        {
            // TODO Make this conditional.
            //Debug.WriteLine();
            //Debug.WriteLine($"Cell({cellRow},{cellColumn}), {nameof(digit)} = {digit}");

            for (int i = 0; i < 9; i++)
            {
                // Check along column at cell.
                if (((table.Rows[testCell.Row][i]) as CellContent).Digit == digit)
                    return false;

                // Check along row at cell.
                if (((table.Rows[i][testCell.Column]) as CellContent).Digit == digit)
                    return false;
            }

            var boxStart = new CellLocation(testCell.Row - (testCell.Row % 3), testCell.Column - (testCell.Column % 3));

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

                    if (((table.Rows[boxCellRow][boxcellColumn]) as CellContent).Digit == digit)
                        return false;
                }
            }

            // No conflicts for digit.
            return true;
        }
    }
}
