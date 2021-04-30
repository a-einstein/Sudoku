namespace Sudoku
{
    public class CellContent
    {
        public int Digit;
        public bool Original;

        public CellContent(int digit)
        {
            // Change to int?.
            Digit = digit;
            Original = (digit != 0) ? true : false;
        }

        public override string ToString()
        {
            return Digit.ToString();
        }
    }
}
