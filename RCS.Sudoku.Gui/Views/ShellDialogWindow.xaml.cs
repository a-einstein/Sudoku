using MahApps.Metro.Controls;
using RCS.Sudoku.Gui.Contracts.Views;
using RCS.Sudoku.Gui.ViewModels;
using System.Windows.Controls;

namespace RCS.Sudoku.Gui.Views
{
    public partial class ShellDialogWindow : MetroWindow, IShellDialogWindow
    {
        public ShellDialogWindow(ShellDialogViewModel viewModel)
        {
            InitializeComponent();
            viewModel.SetResult = OnSetResult;
            DataContext = viewModel;
        }

        public Frame GetDialogFrame()
            => dialogFrame;

        private void OnSetResult(bool? result)
        {
            DialogResult = result;
            Close();
        }
    }
}
