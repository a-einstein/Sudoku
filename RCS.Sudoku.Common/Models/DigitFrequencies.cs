using System.Collections.Generic;
using System.Linq;

namespace RCS.Sudoku.Common.Models
{
    public class DigitFrequencies : Dictionary<int, int>
    {
        /// <summary>
        /// Helper class to record frequency per digit.
        /// </summary>
        public DigitFrequencies()
        {
            // Prepare list.
            for (int digit = 1; digit <= 9; digit++)
            {
                Add(digit, 0);
            }
        }

        /// <summary>
        /// Copy list.
        /// </summary>
        /// <param name="dictionary">List to be copied.</param>
        private DigitFrequencies(DigitFrequencies dictionary)
            : base(dictionary)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A copy of the list.</returns>
        public DigitFrequencies Copy()
        {
            // Note this creates an actual copy because it involves value types.
            return new DigitFrequencies(this);
        }

        /// <summary>
        /// Get sorted state.
        /// </summary>
        /// <returns>Sorted list of digits depending on their recorded frequencies.</returns>
        public int[] SortedDigits()
        {
            return this.OrderByDescending(element => element.Value).ToDictionary(element => element.Key, x => x.Value).Keys.ToArray();
        }
    }
}
