using System;
using System.Collections.ObjectModel;
using System.Data;
using App2.Contracts.ViewModels;
using App2.Core.Contracts.Services;
using App2.Core.Models;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace App2.ViewModels
{
    public class DataGridViewModel : ViewModelBase, INavigationAware
    {
        [PreferredConstructor]
        public DataGridViewModel()
        {
            Init();
        }

        //private readonly ISampleDataService _sampleDataService;

        //public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();
        public DataView Source { get; set; }

        //public DataGridViewModel(ISampleDataService sampleDataService)
        //{
        //    _sampleDataService = sampleDataService;
        //}

        static DataTable table;
        static DataView view;

        private void Init()
        {
            table = new DataTable();

            // Impovised numbering of rows. Needs name.
            // Note this changes indexing.
            table.Columns.Add(new DataColumn("rij", typeof(char)));

            table.Columns.Add(new DataColumn("a", typeof(int)));
            table.Columns.Add(new DataColumn("b", typeof(int)));
            table.Columns.Add(new DataColumn("c", typeof(int)));
            table.Columns.Add(new DataColumn("d", typeof(int)));
            table.Columns.Add(new DataColumn("e", typeof(int)));
            table.Columns.Add(new DataColumn("f", typeof(int)));
            table.Columns.Add(new DataColumn("g", typeof(int)));
            table.Columns.Add(new DataColumn("h", typeof(int)));
            table.Columns.Add(new DataColumn("i", typeof(int)));

            table.Rows.Add('a', 1, 2, 3, 4, 5, 6, 7, 8, 9);
            table.Rows.Add('b', 2, 3, 4, 5, 6, 7, 8, 9, 1);
            table.Rows.Add('c', 3, 4, 5, 6, 7, 8, 9, 1, 2);
            table.Rows.Add('d', 4, 5, 6, 7, 8, 9, 1, 2, 3);
            table.Rows.Add('e', 5, 6, 7, 8, 9, 1, 2, 3, 4);
            table.Rows.Add('f', 6, 7, 8, 9, 1, 2, 3, 4, 5);
            table.Rows.Add('g', 7, 8, 9, 1, 2, 3, 4, 5, 6);
            table.Rows.Add('h', 8, 9, 1, 2, 3, 4, 5, 6, 7);
            table.Rows.Add('i', 9, 1, 2, 3, 4, 5, 6, 7, 8);

            Source = view = table.DefaultView;
        }

        public async void OnNavigatedTo(object parameter)
        {
            //Source.Clear();

            //// TODO WTS: Replace this with your actual data
            //var data = await _sampleDataService.GetGridDataAsync();

            //foreach (var item in data)
            //{
            //    Source.Add(item);
            //}

            //table.Rows[1].ItemArray[1] = 0;
            //table.Rows[1].Field<int>(1) = 0;
            //table.Field<int>(1,1) = 0;

            view[1].Row[8] = 0;
            table.Rows[2][8] = 0;
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
