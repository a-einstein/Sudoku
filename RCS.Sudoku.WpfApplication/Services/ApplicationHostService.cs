﻿using GalaSoft.MvvmLight.Ioc;
using RCS.Sudoku.WpfApplication.Contracts.Services;
using RCS.Sudoku.WpfApplication.Contracts.Views;
using RCS.Sudoku.WpfApplication.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RCS.Sudoku.WpfApplication.Services
{
    public class ApplicationHostService : IApplicationHostService
    {
        private readonly INavigationService _navigationService;
        private readonly IPersistAndRestoreService _persistAndRestoreService;
        private IShellWindow _shellWindow;

        public ApplicationHostService(INavigationService navigationService, IPersistAndRestoreService persistAndRestoreService)
        {
            _navigationService = navigationService;
            _persistAndRestoreService = persistAndRestoreService;
        }

        public async Task StartAsync()
        {
            // Initialize services that you need before app activation
            await InitializeAsync();

            await HandleActivationAsync();

            // Tasks after activation
            await StartupAsync();
        }

        public async Task StopAsync()
        {
            _persistAndRestoreService.PersistData();
            await Task.CompletedTask;
        }

        private async Task InitializeAsync()
        {
            _persistAndRestoreService.RestoreData();
            await Task.CompletedTask;
        }

        private async Task StartupAsync()
        {
            await Task.CompletedTask;
        }

        private async Task HandleActivationAsync()
        {
            if (App.Current.Windows.OfType<IShellWindow>().Count() == 0)
            {
                // Default activation that navigates to the apps default page
                _shellWindow = SimpleIoc.Default.GetInstance<IShellWindow>(Guid.NewGuid().ToString());
                _navigationService.Initialize(_shellWindow.GetNavigationFrame());
                _shellWindow.ShowWindow();

                _navigationService.NavigateTo(typeof(SudokuViewModel).FullName);

                await Task.CompletedTask;
            }
        }
    }
}
