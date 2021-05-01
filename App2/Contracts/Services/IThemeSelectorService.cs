using RCS.Sudoku.Gui.Models;

namespace RCS.Sudoku.Gui.Contracts.Services
{
    public interface IThemeSelectorService
    {
        void InitializeTheme();

        void SetTheme(AppTheme theme);

        AppTheme GetCurrentTheme();
    }
}
