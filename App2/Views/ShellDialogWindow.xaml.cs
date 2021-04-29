using System.Windows.Controls;

using App2.Contracts.Views;
using App2.ViewModels;

using MahApps.Metro.Controls;

namespace App2.Views
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
