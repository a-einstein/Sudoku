﻿using RCS.Sudoku.Common.Contracts.Models;
using RCS.Sudoku.Common.Models;

namespace RCS.Sudoku.ConsoleApplication.Models
{
    public class CellGrid : ICellGrid
    {
        #region Construction
        public CellGrid(Cell[][] grid)
        {
            this.grid = grid;
        }

        private Cell[][] grid;
        #endregion

        #region ICellGrid
        public Cell this[int rowIndex, int columnIndex]
        {
            get => grid[rowIndex][columnIndex];
            set => grid[rowIndex][columnIndex] = value;
        }

        public Cell[] this[int rowIndex]
        {
            get => grid[rowIndex];
            set => grid[rowIndex] = value;
        }

        public void AssignValue(Cell cell, int? digit)
        {
            cell.Digit = digit;
        }
        #endregion
    }
}
