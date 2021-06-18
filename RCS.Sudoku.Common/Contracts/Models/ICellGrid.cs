using RCS.Sudoku.Common.Models;

namespace RCS.Sudoku.Common.Contracts.Models
{
    /// <summary>
    /// Generalization for all applications.
    /// </summary>
    public interface ICellGrid
    {
        Cell this[int rowIndex, int columnIndex] { get; set; }

        // TODO This may be a temporary solution.
        void Assign(Cell cell, int? digit);
    }
}
