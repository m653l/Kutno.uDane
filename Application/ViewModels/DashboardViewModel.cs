﻿using Application.Models;
using Application.Services.Interfaces;
using Application.Stores;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Aggregates;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Themes;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public partial class DashboardViewModel : CoreViewModel
    {
        public static List<FilePickerFileType> Types = new List<FilePickerFileType>();
        public static FilePickerFileType XmlType { get; } = new("All Images")
        {
            Patterns = new[] { "*.xml", "*.xlsx", "*.xlsm" }
        };
        public ObservableCollection<PilotInfo> SchoolsInfo { get; set; }

        [ObservableProperty]
        private RowSeries<PilotInfo> _series;
        [ObservableProperty]
        private Axis[] _xAxes = { new Axis { SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220)) } };

        [ObservableProperty]
        private Axis[] _yAxes = { new Axis { IsVisible = false } };

        [ObservableProperty]
        private string _sioFilePath = string.Empty;
        [ObservableProperty]
        private string _schoolsFilePath = string.Empty;
        [ObservableProperty]
        private string _expensesFilePath = string.Empty;
        [ObservableProperty]
        private string _incomesFilePath = string.Empty;

        SolidColorPaint[] _paints;

        private readonly IImportDataService _importDataService;
        private readonly ApplicationDataStore _applicationDataStore;
        public DashboardViewModel(IServiceProvider serviceProvider, IImportDataService importDataService) : base(serviceProvider)
        {
            Types.Add(XmlType);

            _importDataService = importDataService;

            Series = new RowSeries<PilotInfo>
            {
                Values = SchoolsInfo,
                DataLabelsPaint = new SolidColorPaint(new SKColor(245, 245, 245)),
                DataLabelsPosition = DataLabelsPosition.End,
                DataLabelsTranslate = new(-1, 0),
                DataLabelsFormatter = point => $"{point.Model!.Name} {point.Coordinate.PrimaryValue}",
                MaxBarWidth = 50,
                Padding = 10,
            };
        }
        [RelayCommand]
        public async Task PickSio(Control view)
        {
            SioFilePath = await PickFileAsync(view);
        }
        [RelayCommand]
        public async Task PickSchools(Control view)
        {
            SchoolsFilePath = await PickFileAsync(view);
        }
        [RelayCommand]
        public async Task PickExpenses(Control view)
        {
            ExpensesFilePath = await PickFileAsync(view);
        }
        [RelayCommand]
        public async Task PickIncomes(Control view)
        {
            IncomesFilePath = await PickFileAsync(view);
        }
        [RelayCommand]
        public async Task ReadData()
        {
            _importDataService.ImportExamsData(SchoolsFilePath);
            _importDataService.ImportSioData(SioFilePath);
            _importDataService.ImportIncome(IncomesFilePath);
            SetUpChart();
        }

        private async Task<string> PickFileAsync(Control view)
        {
            var topLevel = TopLevel.GetTopLevel(view);

            // Start async operation to open the dialog.
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                AllowMultiple = false,
                FileTypeFilter = Types
            });

            if (files.Count >= 1)
            {
                //// Open reading stream from the first file.
                //await using var stream = await files[0].OpenReadAsync();
                //using var streamReader = new StreamReader(stream);
                //// Reads all the content of file as a text.
                //var fileContent = await streamReader.ReadToEndAsync();

                return files[0].Path.ToString().Replace("file:///", "");
            }

            return string.Empty;
        }

        private void SetUpChart()
        {
            _paints = Enumerable.Range(0, _applicationDataStore.Schools.Count)
            .Select(i => new SolidColorPaint(ColorPalletes.MaterialDesign500[i].AsSKColor()))
            .ToArray();

            for (int i = 0; i < _applicationDataStore.Schools.Count; i++)
            {
                SchoolsInfo.Add(new PilotInfo(_applicationDataStore.Schools[i].Name, (int)_applicationDataStore.Schools[i].StudentCount, _paints[i]));
            }
        }
    }
}
