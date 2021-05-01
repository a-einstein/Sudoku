﻿namespace RCS.Sudoku.Common
{
    public struct CellLocation
    {
        public int Row;
        public int Column;

        public CellLocation(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
