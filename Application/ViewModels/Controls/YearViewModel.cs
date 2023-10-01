using Application.Models;
using Application.Services;
using Application.Services.Interfaces;
using Application.Stores;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Aggregates;
using LiveChartsCore;
using LiveChartsCore.ConditionalDraw;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Themes;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace Application.ViewModels.Controls
{
    public partial class YearViewModel : CoreViewModel
    {
        public static List<FilePickerFileType> Types = new List<FilePickerFileType>();
        public static FilePickerFileType XmlType { get; } = new("All Images")
        {
            Patterns = new[] { "*.xml", "*.xlsx", "*.xlsm" }
        };
        public ObservableCollection<PilotInfo> SchoolsInfo { get; set; } = new();
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private ISeries[] _series;

        [ObservableProperty]
        private Axis[] _xAxes = { new Axis { SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220)) } };

        [ObservableProperty]
        private Axis[] _yAxes = { new Axis { IsVisible = false, } };

        SolidColorPaint[] _paints;

        public ObservableCollection<School> Schools { get; set; } = new();

        [ObservableProperty]
        private string _sioFilePath = string.Empty;

        [ObservableProperty]
        private string _schoolsFilePath = string.Empty;

        [ObservableProperty]
        private string _expensesFilePath = string.Empty;

        [ObservableProperty]
        private string _incomesFilePath = string.Empty;

        private readonly IImportDataService _importDataService;
        private readonly IPopupService _popupService;
        private readonly ApplicationDataStore _applicationDataStore;
        private readonly ShellViewModel _shellViewModel;
        public YearViewModel(ApplicationDataStore applicationDataStore, IServiceProvider serviceProvider, IImportDataService importDataService, IPopupService popupService, ShellViewModel shellViewModel) : base(serviceProvider)
        {
            Types.Add(XmlType);

            _applicationDataStore = applicationDataStore;
            _importDataService = importDataService;
            _popupService = popupService;

            Name = DateTime.Now.Year.ToString();

            var rowSeries = (RowSeries<PilotInfo>)new RowSeries<PilotInfo>
            {
                Values = SchoolsInfo,
                DataLabelsPaint = new SolidColorPaint(new SKColor(0, 0, 0)),
                DataLabelsPosition = DataLabelsPosition.End,
                DataLabelsSize = 14,
                DataLabelsMaxWidth = 1000,
                DataLabelsTranslate = new(-1, 0),
                DataLabelsFormatter = point => $"{point.Model!.Name}",
                MaxBarWidth = 5000,
                Padding = 10,
            }.OnPointMeasured(point =>
            {
                // assign a different color to each point
                if (point.Visual is null) return;
                point.Visual.Fill = point.Model!.Paint;
            });

            _series = new[] { rowSeries };
            _shellViewModel = shellViewModel;
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
            if (SioFilePath is "" || SchoolsFilePath is "" || ExpensesFilePath is "" || IncomesFilePath is "")
            {
                ErrorDialogViewModel vm = _serviceProvider.GetRequiredService<ErrorDialogViewModel>();
                vm.Title = "Błąd walidacji";
                vm.Message = "Aby kontynuować wybierz wszystkie pliki!";
                _popupService.OpenPopup(vm);
            }
            else
            {
                _shellViewModel.ActivateButtons();
                _importDataService.ImportData(this, SioFilePath, SchoolsFilePath, ExpensesFilePath, IncomesFilePath);
                SetUpChart();
            }
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
            Series[0].Values = Enumerable.Empty<PilotInfo>();
            SchoolsInfo = new ObservableCollection<PilotInfo>();

            var rowSeries = (RowSeries<PilotInfo>)new RowSeries<PilotInfo>
            {
                Values = SchoolsInfo,
                DataLabelsPaint = new SolidColorPaint(new SKColor(0, 0, 0)),
                DataLabelsPosition = DataLabelsPosition.End,
                DataLabelsSize = 14,
                DataLabelsMaxWidth = 1000,
                DataLabelsTranslate = new(-1, 0),
                DataLabelsFormatter = point => $"{point.Model!.Name}",
                MaxBarWidth = 5000,
                Padding = 10,
            }.OnPointMeasured(point =>
            {
                // assign a different color to each point
                if (point.Visual is null) return;
                point.Visual.Fill = point.Model!.Paint;
            });

            _series = new[] { rowSeries };

            _paints = Enumerable.Range(0, SchoolsInfo.Count)
            .Select(i => new SolidColorPaint(ColorPalletes.MaterialDesign500[i % ColorPalletes.MaterialDesign500.Count()].AsSKColor()))
            .ToArray();

            for (int i = 0; i < SchoolsInfo.Count; i++)
            {
                SchoolsInfo.Add(new PilotInfo(SchoolsInfo[i].Name, SchoolsInfo[i].Value, _paints[i]));
            }

            Series[0].Values = SchoolsInfo.OrderBy(i => i.Value).ToList();

            _serviceProvider.GetRequiredService<PlotterViewModel>().UpdateCharts();

            OnPropertyChanged(nameof(SchoolsInfo));
        }
        [RelayCommand]
        public void DeleteYear()
        {
            _applicationDataStore.Years.Remove(this);
        }
    }
}
