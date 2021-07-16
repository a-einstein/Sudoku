namespace RCS.Sudoku.Common.Models
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
        public Cell(int? digit = null)
        {
            Digit = digit != 0 ? digit : null;
            Original = Digit.HasValue;
        }

        /// <summary>
        /// Display cell value (accommodating TableView).
        /// </summary>
        /// <returns>Digit value or empty string if cell is empty.</returns>
        public override string ToString()
        {
            return ToString(string.Empty);
        }

        /// <summary>
        /// Display cell value (general).
        /// </summary>
        /// <param name="nullProxy">String to represent null value. Defaults to 0.</param>
        /// <returns>Digit value or nullProxy.</returns>
        public string ToString(string nullProxy = default)
        {
            if (nullProxy == default)
                // Note this results in 0 for null.
                return Digit.GetValueOrDefault().ToString();
            else
                return
                    Digit.HasValue
                    ? Digit.ToString()
                    : nullProxy;
        }
    }
}
