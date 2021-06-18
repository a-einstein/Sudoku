using RCS.Sudoku.Common.Contracts.Models;
using RCS.Sudoku.Common.Models;
using RCS.Sudoku.Common.Properties;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RCS.Sudoku.Common.Services
{
    /// <summary>
    /// Core class with essential action on the puzzle itself.
    /// </summary>
    public class SudokuService
    {
        /// <summary>
        /// Sorted list of digits depending on their frequencies.
        /// </summary>
        private int[] sortedDigits;

        /// <summary>
        /// Read file and do some validity checks. Assumes a 9x9 textual grid with 0 in empty cells.
        /// Also assemble additional information for solving.
        /// </summary>
        /// <param name="message">Resulting message with either the file name or error report.</param>
        /// <param name="grid">Resulting grid.</param>
        /// <returns>Success or failure.</returns>
        public bool Read(out string message, out Cell[][] grid)
        {
            var initialDirectory = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\..\\puzzles");

            var fileDialog = new OpenFileDialog
            {
                Title = Resources.FileReadTitle,
                Filter = "TXT files|*.txt",
                InitialDirectory = initialDirectory
            };

            // Use jagged array for (supposed) speed and transferability.
            grid = new Cell[9][];

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = fileDialog.FileName;
                Trace.WriteLine($"File = '{filePath}'.");

                message = string.Format(Resources.MessageFileName_name, Path.GetFileName(filePath));
                return ProcessFile(filePath, ref message, ref grid);
            }
            else
            {
                message = "No file read.";
                return false;
            }
        }

        /// <summary>
        /// Read a file into a grid and analyse contents.
        /// </summary>
        /// <param name="filePath">Path to file to be read.</param>
        /// <param name="message">Resulting messages.</param>
        /// <param name="grid">Resulting grid.</param>
        /// <returns>Success.</returns>
        private bool ProcessFile(string filePath, ref string message, ref Cell[][] grid)
        {
            string[] fileLines = File.ReadAllLines(filePath);

            if (fileLines.Length != 9)
            {
                message = string.Format(Resources.ErrorPuzzleRows_rows, 9);
                Trace.WriteLine(message);
                return false;
            }

            // Frequency of digits present in the initial sudoku.
            var digitFrequencies = new DigitFrequencies();

            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                var fileLine = fileLines[rowIndex];

                // Only keep digits.
                var line = Regex.Replace(fileLine, @"\D", "");

                if (line.Length != 9)
                {
                    message = string.Format(Resources.ErrorRowDigits_row_digits, rowIndex + 1, 9);
                    Trace.WriteLine(message);
                    return false;
                }

                grid[rowIndex] = new Cell[9];

                for (int columnIndex = 0; columnIndex < 9; columnIndex++)
                {
                    // Currently redundant as the line should be filtered.
                    if (int.TryParse(line[columnIndex].ToString(), out int digit))
                    {
                        grid[rowIndex][columnIndex] = new Cell(digit);

                        if (digit != 0)
                            digitFrequencies[digit]++;
                    }
                    else
                    {
                        message = string.Format(Resources.ErrorRowNonDigits_row, rowIndex + 1);
                        Trace.WriteLine(message);
                        return false;
                    }
                }
            }

            sortedDigits = digitFrequencies.SortedDigits();

            return true;
        }

        public ICellGrid Grid { get;  set; }

        /// <summary>
        /// Core recursive solution function.
        /// </summary>
        /// <param name="rowIndex">Startposition.</param>
        /// <param name="columnIndex">Startposition.</param>
        /// 
        /// <returns>Success or failure.</returns>
        public ActionStatus CompleteFrom(int rowIndex, int columnIndex)
        {
            var cell = Grid[rowIndex, columnIndex];

            // Cell HAS a value.
            if (cell.Digit.HasValue)
            {
                // Row not completed.
                if (++columnIndex < 9)
                {
                    // Next cell in row.
                    return CompleteFrom(rowIndex, columnIndex);
                }
                // Rows not completed.
                else if (++rowIndex < 9)
                {
                    // Start on next row.
                    return CompleteFrom(rowIndex, 0);
                }
                // All completed from start.
                else
                {
                    return ActionStatus.Succeeded;
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
                    if (DigitAvailableForCell(digit, new CellLocation(rowIndex, columnIndex), Grid))
                    {
                        // Try digit in cell.
                        Grid.Assign(cell, digit);

                        // Row not completed.
                        if ((columnIndex + 1) < 9)
                        {
                            // Next cell in row.
                            if (CompleteFrom(rowIndex, columnIndex + 1) == ActionStatus.Succeeded)
                                // No conflicts encountered for digit in remainder of grid.
                                return ActionStatus.Succeeded;
                            else
                            {
                                // Backtrack. Next digit.
                                Grid.Assign(cell, null);
                            }

                        }
                        // Rows not completed.
                        else if ((rowIndex + 1) < 9)
                        {
                            // Next row.
                            if (CompleteFrom(rowIndex + 1, 0) == ActionStatus.Succeeded)
                                // No conflicts encountered for digit in remainder of grid.
                                return ActionStatus.Succeeded;
                            else
                            {
                                // Backtrack. Next digit.
                                Grid.Assign(cell, null);
                            }
                        }
                        // No conflicts encountered for digit in remainder of grid.
                        else
                        {
                            return ActionStatus.Succeeded;
                        }
                    }
                }
            }

            // No completion for cell.
            return ActionStatus.Failed;
        }

        /// <summary>
        /// Core function to consider whether digit is avalaible for a location.
        /// Consider row, column, and box of a location.
        /// </summary>
        /// <param name="digit">Considered digit.</param>
        /// <param name="testLocation">Considered location.</param>
        /// <param name="grid">Containing data structure.</param>
        /// <returns></returns>
        private bool DigitAvailableForCell(int digit, CellLocation testLocation, ICellGrid grid)
        {
            // TODO Make this conditional.
            //Debug.WriteLine();
            //Debug.WriteLine($"Cell({cellRow},{cellColumn}), {nameof(digit)} = {digit}");

            for (int i = 0; i < 9; i++)
            {
                // Check along column at cell.
                if (((grid[testLocation.RowIndex, i]) as Cell).Digit == digit)
                    return false;

                // Check along row at cell.
                if (((grid[i, testLocation.ColumnIndex]) as Cell).Digit == digit)
                    return false;
            }

            var boxStartLocation = new CellLocation(testLocation.RowIndex - (testLocation.RowIndex % 3), testLocation.ColumnIndex - (testLocation.ColumnIndex % 3));

            // Check remainder of box.
            for (int boxCellRowIndex = boxStartLocation.RowIndex; boxCellRowIndex < boxStartLocation.RowIndex + 3; boxCellRowIndex++)
            {
                // Skip row already tested (hardly gaining).
                if (boxCellRowIndex == testLocation.RowIndex)
                    continue;

                for (int boxcellColumnIndex = boxStartLocation.ColumnIndex; boxcellColumnIndex < boxStartLocation.ColumnIndex + 3; boxcellColumnIndex++)
                {
                    // Skip column already tested (hardly gaining).
                    if (boxcellColumnIndex == testLocation.ColumnIndex)
                        continue;

                    //Debug.WriteLine($"boxCell({boxCellRow},{boxcellColumn}) = {puzzle[boxCellRow, boxcellColumn]}");

                    if (((grid[boxCellRowIndex, boxcellColumnIndex]) as Cell).Digit == digit)
                        return false;
                }
            }

            // No conflicts for digit.
            return true;
        }
    }
}
