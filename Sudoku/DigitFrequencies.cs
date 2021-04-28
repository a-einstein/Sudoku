using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    internal class DigitFrequencies : Dictionary<int, int>
    {
        public DigitFrequencies()
        {
            for (int digit = 1; digit <= 9; digit++)
            {
                Add(digit, 0);
            }
        }

        private DigitFrequencies(DigitFrequencies dictionary)
            : base(dictionary)
        { }

        public DigitFrequencies Copy()
        {
            // Note this creates an actual copy because it involves value types.
            return new DigitFrequencies(this);
        }

        public int[] SortedDigits()
        {
            return this.OrderByDescending(element => element.Value).ToDictionary(element => element.Key, x => x.Value).Keys.ToArray();
        }
    }
}
