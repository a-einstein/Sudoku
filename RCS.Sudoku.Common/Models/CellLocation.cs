namespace RCS.Sudoku.Common.Models
{
    /// <summary>
    /// Helper container to compactly store cell location.
    /// </summary>
    /// 
    // Note this record syntax creates init-only auto-implemented properties.
    public record CellLocation(int RowIndex, int ColumnIndex);
}
