using System;
using System.Collections.ObjectModel;

using App2.Contracts.ViewModels;
using App2.Core.Contracts.Services;
using App2.Core.Models;

using GalaSoft.MvvmLight;

namespace App2.ViewModels
{
    public class DataGridViewModel : ViewModelBase, INavigationAware
    {
        private readonly ISampleDataService _sampleDataService;

        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public DataGridViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await _sampleDataService.GetGridDataAsync();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
