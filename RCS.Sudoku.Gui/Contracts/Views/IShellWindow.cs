using System.Windows.Controls;

namespace RCS.Sudoku.Gui.Contracts.Views
{
    public interface IShellWindow
    {
        Frame GetNavigationFrame();

        void ShowWindow();

        void CloseWindow();
    }
}
