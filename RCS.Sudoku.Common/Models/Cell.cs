namespace RCS.Sudoku.Common
{
    public class Cell
    {
        /// <summary>
        /// Value of cell.
        /// </summary>
        public int? Digit { get; set; }

        /// <summary>
        /// Part of original clues?
        /// </summary>
        public bool Original { get; }

        /// <summary>
        /// Construct. Only way to set Original.
        /// </summary>
        /// <param name="digit">Value to assign.</param>
        public Cell(int? digit)
        {
            Digit = digit != 0 ? digit : null;
            Original = digit != 0;
        }

        /// <summary>
        /// Display cell value.
        /// </summary>
        /// <returns>Digit value or null if cell is empty.</returns>
        public override string ToString()
        {
            return Digit.ToString();
        }

        /// <summary>
        /// Display cell value in textual application.
        /// </summary>
        /// <param name="showZeros">Choose to display empty cell as zeros.</param>
        /// <returns></returns>
        public string ToString(bool showZeros)
        {
            if (showZeros)
                return Digit.GetValueOrDefault().ToString();
            else
                return ToString();
        }
    }
}
