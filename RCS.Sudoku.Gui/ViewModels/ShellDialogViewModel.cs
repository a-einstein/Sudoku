using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;

namespace RCS.Sudoku.Gui.ViewModels
{
    public class ShellDialogViewModel : ViewModelBase
    {
        private ICommand _closeCommand;

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(OnClose));

        public Action<bool?> SetResult { get; set; }

        public ShellDialogViewModel()
        {
        }

        private void OnClose()
        {
            bool result = true;
            SetResult(result);
        }
    }
}
