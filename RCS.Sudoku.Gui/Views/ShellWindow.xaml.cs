using MahApps.Metro.Controls;
using RCS.Sudoku.Gui.Contracts.Views;
using System.Windows.Controls;

namespace RCS.Sudoku.Gui.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow
    {
        public ShellWindow()
        {
            InitializeComponent();
        }

        public Frame GetNavigationFrame()
            => shellFrame;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();
    }
}
