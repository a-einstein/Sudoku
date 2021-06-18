using RCS.Sudoku.Common.Models;

namespace RCS.Sudoku.Common.Contracts.Models
{
    /// <summary>
    /// Generalization for all applications.
    /// </summary>
    public interface ICellGrid
    {
        /// <summary>
        /// Get or set cell.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        Cell this[int rowIndex, int columnIndex] { get; set; }

        /// <summary>
        /// Get or set row.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        Cell[] this[int rowIndex] { get; set; }

        /// <summary>
        /// Assign value into cell.
        /// Handle event by grid.
        /// </summary>
        /// <param name="cell">Receiving cell.</param>
        /// <param name="digit">Value to assign.</param>
        void AssignValue(Cell cell, int? digit);
    }
}
