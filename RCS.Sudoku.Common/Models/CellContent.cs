namespace RCS.Sudoku.Common
{
    public class CellContent
    {
        public int? Digit { get; set; }
        public bool Original { get; }

        public CellContent(int? digit)
        {
            Digit = digit != 0 ? digit : null;
            Original = digit != 0;
        }

        public override string ToString()
        {
            return Digit.ToString();
        }

        public string ToString(bool showZeros)
        {
            if (showZeros)
                return Digit.GetValueOrDefault().ToString();
            else
                return Digit.ToString();
        }
    }
}
