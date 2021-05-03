using System.Windows.Controls;

namespace RCS.Sudoku.WpfApplication.Contracts.Views
{
    public interface IShellWindow
    {
        Frame GetNavigationFrame();

        void ShowWindow();

        void CloseWindow();
    }
}
