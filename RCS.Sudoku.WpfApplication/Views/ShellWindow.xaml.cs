using RCS.Sudoku.WpfApplication.Contracts.Views;
using System.Windows;
using System.Windows.Controls;

namespace RCS.Sudoku.WpfApplication.Views
{
    public partial class ShellWindow : Window, IShellWindow
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
