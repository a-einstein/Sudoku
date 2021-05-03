using RCS.Sudoku.WpfApplication.Models;

namespace RCS.Sudoku.WpfApplication.Contracts.Services
{
    public interface IThemeSelectorService
    {
        void InitializeTheme();

        void SetTheme(AppTheme theme);

        AppTheme GetCurrentTheme();
    }
}
