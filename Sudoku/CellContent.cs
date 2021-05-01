namespace Sudoku
{
    public class CellContent
    {
        public int Digit { get; set; }
        public bool Original { get; }

        public CellContent(int digit)
        {
            // Change to int?.
            Digit = digit;
            Original = digit != 0;
        }

        public override string ToString()
        {
            return Digit.ToString();
        }
    }
}
