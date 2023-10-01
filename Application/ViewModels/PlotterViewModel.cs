using Application.Services.Interfaces;
using Application.Models;
using Application.Stores;
using Application.ViewModels.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.ConditionalDraw;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Themes;
using SkiaSharp;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public partial class PlotterViewModel : CoreViewModel
    {
        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }

        private int _selectedIndexCombo = 0;
        public int SelectedIndexCombo
        {
            get => _selectedIndexCombo;
            set
            {
                _selectedIndexCombo = value;
                OnPropertyChanged();
                UpdateChart(value);
            }
        }

        public ObservableCollection<YearViewModel> Years
        {
            get => _applicationDataStore.Years;
        }

        public List<ISeries> SaldosSeries { get; set; }
        public List<ISeries> SaldosPerStudentSeries { get; set; }
        public List<ISeries> StalinSeries { get; set; }
        public List<ISeries> CostPerStalinSeries { get; set; }

        SolidColorPaint[] _paints;

        [ObservableProperty]
        private Axis[] _xAxes = { new Axis { IsVisible = false } };
        [ObservableProperty]
        private Axis[] _yAxes = { new Axis { IsVisible = false, } };

        public IRelayCommand OpenInfoPopupCommand { get; set; }

        private readonly ApplicationDataStore _applicationDataStore;
        private readonly IPopupService _popupService;
        public PlotterViewModel(ApplicationDataStore applicationDataStore, IServiceProvider serviceProvider, IPopupService popupService) : base(serviceProvider)
        {
            _applicationDataStore = applicationDataStore;

            _applicationDataStore.ActiveYear = _applicationDataStore.Years.FirstOrDefault();
            XAxes[0].Labels = _applicationDataStore.ActiveYear.Schools.Select(i => i.Name).ToList();

            var saldos = new List<PilotInfo>();
            var saldosPerStudent = new List<PilotInfo>();
            var stalin = new List<PilotInfo>();
            var costPerStalin = new List<PilotInfo>();

            OpenInfoPopupCommand = new RelayCommand(OpenInfoPopup);

            for (int i = 0; i < applicationDataStore.ActiveYear.Schools.Count; i++)
            _paints = Enumerable.Range(0, _applicationDataStore.ActiveYear.Schools.Count)
                    .Select(i => new SolidColorPaint(ColorPalletes.MaterialDesign500[i % ColorPalletes.MaterialDesign500.Count()].AsSKColor()))
                    .ToArray();

            for (int i = 0; i < applicationDataStore.ActiveYear.Schools.Count; i++)
            {
                saldos.Add(new PilotInfo(applicationDataStore.ActiveYear.Schools[i].Name, (double)applicationDataStore.ActiveYear.Schools[i].Saldo(), _paints[i]));
                saldosPerStudent.Add(new PilotInfo(applicationDataStore.ActiveYear.Schools[i].Name, (double)applicationDataStore.ActiveYear.Schools[i].SaldoPerStudent(), _paints[i]));
                stalin.Add( new PilotInfo(applicationDataStore.ActiveYear.Schools[i].Name, (double)applicationDataStore.ActiveYear.Schools[i].GetTrzyStaliny(), _paints[i]));
                costPerStalin.Add( new PilotInfo(applicationDataStore.ActiveYear.Schools[i].Name, (double)applicationDataStore.ActiveYear.Schools[i].GetCostPerStanin(), _paints[i]));
            }

            var columnSeries1 = new ColumnSeries<PilotInfo>
            {
                Values = saldos,
                Stroke = null,
                Padding = 2,

            }.OnPointMeasured(point =>
            {
                // assign a different color to each point
                if (point.Visual is null) return;
                point.Visual.Fill = point.Model!.Paint;
            });

            var columnSeries2 = new ColumnSeries<PilotInfo>
            {
                Values = saldosPerStudent,
                Stroke = null,
                Padding = 2
            }.OnPointMeasured(point =>
            {
                // assign a different color to each point
                if (point.Visual is null) return;
                point.Visual.Fill = point.Model!.Paint;
            });

            var columnSeries3 = new ColumnSeries<PilotInfo>
            {
                Values = stalin,
                Stroke = null,
                Padding = 2
            }.OnPointMeasured(point =>
            {
                // assign a different color to each point
                if (point.Visual is null) return;
                point.Visual.Fill = point.Model!.Paint;
            });

            var columnSeries4 = new ColumnSeries<PilotInfo>
            {
                Values = costPerStalin,
                Stroke = null,
                Padding = 2
            }.OnPointMeasured(point =>
            {
                // assign a different color to each point
                if (point.Visual is null) return;
                point.Visual.Fill = point.Model!.Paint;
            });

            //var columnSeries3 = new ColumnSeries<decimal>
            //{
            //    Values = values3,
            //    Stroke = null,
            //    Padding = 2
            //};

            SaldosSeries = new List<ISeries> { columnSeries1 };
            SaldosPerStudentSeries = new List<ISeries> { columnSeries2 };
            StalinSeries = new List<ISeries> { columnSeries3 };
            CostPerStalinSeries = new List<ISeries> { columnSeries4 };
            _popupService = popupService;
        }

        public void UpdateCharts()
        {
            XAxes[0].Labels = _applicationDataStore.ActiveYear.Schools.Select(i => i.Name).ToList();

            var saldos = new List<decimal>();
            var saldosPerStudent = new List<decimal>();
            var stalin = new List<decimal>();
            var costPerStalin = new List<decimal>();

            for (int i = 0; i < _applicationDataStore.ActiveYear.Schools.Count; i++)
            {
                saldos.Add(_applicationDataStore.ActiveYear.Schools[i].Saldo());
                saldosPerStudent.Add(_applicationDataStore.ActiveYear.Schools[i].SaldoPerStudent());
                stalin.Add(_applicationDataStore.ActiveYear.Schools[i].GetTrzyStaliny());
                costPerStalin.Add(_applicationDataStore.ActiveYear.Schools[i].GetCostPerStanin());
            }

            var columnSeries1 = new ColumnSeries<decimal>
            {
                Values = saldos,
                Stroke = null,
                Padding = 2,

            };

            var columnSeries2 = new ColumnSeries<decimal>
            {
                Values = saldosPerStudent,
                Stroke = null,
                Padding = 2
            };

            var columnSeries3 = new ColumnSeries<decimal>
            {
                Values = stalin,
                Stroke = null,
                Padding = 2
            };

            var columnSeries4 = new ColumnSeries<decimal>
            {
                Values = costPerStalin,
                Stroke = null,
                Padding = 2
            };

            SaldosSeries = new List<ISeries> { columnSeries1 };
            SaldosPerStudentSeries = new List<ISeries> { columnSeries2 };
            StalinSeries = new List<ISeries> { columnSeries3 };
            CostPerStalinSeries = new List<ISeries> { columnSeries4 };
        }

        private void UpdateChart(int newValue)
        {

        }

        private void OpenInfoPopup()
        {
            ErrorDialogViewModel vm = _serviceProvider.GetRequiredService<ErrorDialogViewModel>();

            switch (SelectedIndex)
            {
                case 0:
                    vm.Title = "Saldo";
                    vm.Message = "Saldo jest obliczne w następujący sposób: Subwencja na daną placówkę + Dochód - Wydatki.";
                    _popupService.OpenPopup(vm);
                    break;
                case 1:
                    vm.Title = "Saldo na ucznia";
                    vm.Message = "Jest to ilość pieniędzy z salda przeznaczona na jednogo ucznia.";
                    _popupService.OpenPopup(vm);
                    break;
                case 2:
                    vm.Title = "Staniny";
                    vm.Message = "Suma punków staniny z egzaminów 8-klasisty dla danej szkoły.";
                    _popupService.OpenPopup(vm);
                    break;
                case 3:
                    vm.Title = "Wskaźnik";
                    vm.Message = "Jest to koszt uzyskania jednogo punktu staniny dla danej placówki. (mniej lepiej)";
                    _popupService.OpenPopup(vm);
                    break;
                default:
                    break;
            }
        }
    }
}
