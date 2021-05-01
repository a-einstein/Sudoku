using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RCS.Sudoku.Gui.Contracts.Services;
using System.Windows.Input;

namespace RCS.Sudoku.Gui.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly IRightPaneService _rightPaneService;
        private ICommand _loadedCommand;
        private ICommand _unloadedCommand;

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand(OnUnloaded));

        public ShellViewModel(IRightPaneService rightPaneService)
        {
            _rightPaneService = rightPaneService;
        }

        private void OnLoaded()
        {
        }

        private void OnUnloaded()
        {
            _rightPaneService.CleanUp();
        }
    }
}
