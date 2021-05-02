using RCS.Sudoku.Common;
using System;
using System.Diagnostics;

namespace RCS.Sudoku.Console
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // TODO Output only works in debug mode.
            // Either get working as Trace/Debug, to window or file (see WpfShop too), or create a GUI.
            Debug.WriteLine("Test Debug");
            Trace.WriteLine("Test Trace");

            var taskLine = "===============================";
            Trace.WriteLine(taskLine);

            if (Puzzle.Read())
            {
                Puzzle.Handle();
            }
        }
    }
}
