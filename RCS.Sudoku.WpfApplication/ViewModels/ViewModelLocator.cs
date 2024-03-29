﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Extensions.Configuration;
using RCS.Sudoku.Common.Contracts.Services;
using RCS.Sudoku.Common.Services;
using RCS.Sudoku.WpfApplication.Contracts.Services;
using RCS.Sudoku.WpfApplication.Contracts.Views;
using RCS.Sudoku.WpfApplication.Models;
using RCS.Sudoku.WpfApplication.Services;
using RCS.Sudoku.WpfApplication.Views;
using System.Windows.Controls;

namespace RCS.Sudoku.WpfApplication.ViewModels
{
    public class ViewModelLocator
    {
        private IPageService PageService
            => SimpleIoc.Default.GetInstance<IPageService>();

        public ShellViewModel ShellViewModel
            => SimpleIoc.Default.GetInstance<ShellViewModel>();

        public SudokuViewModel SudokuViewModel
            => SimpleIoc.Default.GetInstance<SudokuViewModel>();

        public ViewModelLocator()
        {
            // App Host
            SimpleIoc.Default.Register<IApplicationHostService, ApplicationHostService>();

            // Core Services
            SimpleIoc.Default.Register<IApplicationInfoService, ApplicationInfoService>();
            SimpleIoc.Default.Register<ISystemService, SystemService>();
            SimpleIoc.Default.Register<IFileService, FileService>();

            // Services
            SimpleIoc.Default.Register<IPersistAndRestoreService, PersistAndRestoreService>();
            SimpleIoc.Default.Register<IPageService, PageService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();

            // Window
            SimpleIoc.Default.Register<IShellWindow, ShellWindow>();
            SimpleIoc.Default.Register<ShellViewModel>();

            // Pages
            Register<SudokuViewModel, SudokuPage>();
        }

        private void Register<VM, V>()
            where VM : ViewModelBase
            where V : Page
        {
            SimpleIoc.Default.Register<VM>();
            SimpleIoc.Default.Register<V>();
            PageService.Configure<VM, V>();
        }

        public void AddConfiguration(IConfiguration configuration)
        {
            var appConfig = configuration
                .GetSection(nameof(AppConfig))
                .Get<AppConfig>();

            // Register configurations to IoC
            SimpleIoc.Default.Register(() => configuration);
            SimpleIoc.Default.Register(() => appConfig);
        }
    }
}
