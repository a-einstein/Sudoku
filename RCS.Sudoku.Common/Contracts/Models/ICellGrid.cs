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

        // TODO This may be a temporary solution.
        void Assign(Cell cell, int? digit);
    }
}
