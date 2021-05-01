using MahApps.Metro.Controls;
using RCS.Sudoku.Gui.Behaviors;
using System.Windows.Controls;

namespace RCS.Sudoku.Gui.Contracts.Views
{
    public interface IShellWindow
    {
        Frame GetNavigationFrame();

        void ShowWindow();

        void CloseWindow();

        Frame GetRightPaneFrame();

        SplitView GetSplitView();

        RibbonTabsBehavior GetRibbonTabsBehavior();
    }
}
