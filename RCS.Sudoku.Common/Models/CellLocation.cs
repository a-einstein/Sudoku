namespace RCS.Sudoku.Common
{
    /// <summary>
    /// Helper structure to compactly store cell location.
    /// </summary>
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
