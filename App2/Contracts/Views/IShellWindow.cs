using System.Windows.Controls;

using App2.Behaviors;

using MahApps.Metro.Controls;

namespace App2.Contracts.Views
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
