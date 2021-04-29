using App2.Contracts.Services;
using App2.Contracts.ViewModels;
using App2.Core.Models;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace App2.ViewModels
{
    public class ContentGridViewModel : ViewModelBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private ICommand _navigateToDetailCommand;

        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public ContentGridViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            Source.Clear();

            // TODO WTS: Replace this with your actual data
         }

        public void OnNavigatedFrom()
        {
        }
    }
}
