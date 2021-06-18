using RCS.Sudoku.Common.Contracts.Models;
using RCS.Sudoku.Common.Models;

namespace RCS.Sudoku.ConsoleApplication.Models
{
    public class CellGrid : ICellGrid
    {
        public CellGrid(Cell[][] grid)
        {
            this.grid = grid;
        }

        private Cell[][] grid;

        public Cell this[int rowIndex, int columnIndex]
        {
            get => grid[rowIndex][columnIndex];
            set => grid[rowIndex][columnIndex] = value;
        }

        // TODO Put this in the interface too? If at all used.
        public Cell[] this[int rowIndex]
        {
            get => grid[rowIndex];
            set => grid[rowIndex] = value;
        }

        public void Assign(Cell cell, int? digit)
        {
            cell.Digit = digit;
        }
    }
}
