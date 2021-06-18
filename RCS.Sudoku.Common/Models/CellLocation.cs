namespace RCS.Sudoku.Common.Models
{
    /// <summary>
    /// Helper structure to compactly store cell location.
    /// </summary>
public struct CellLocation
    {
        public int RowIndex;
        public int ColumnIndex;

        public CellLocation(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }
    }
}
