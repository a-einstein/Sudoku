using System.Windows.Input;

using App2.Contracts.Services;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace App2.ViewModels
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
