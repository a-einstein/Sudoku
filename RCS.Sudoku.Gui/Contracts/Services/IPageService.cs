using GalaSoft.MvvmLight;
using System;
using System.Windows.Controls;

namespace RCS.Sudoku.WpfApplication.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);

        Page GetPage(string key);

        void Configure<VM, V>()
            where VM : ViewModelBase
            where V : Page;
    }
}
