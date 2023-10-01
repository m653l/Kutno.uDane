using Application.Services.Interfaces;
using Application.Stores;
using Application.ViewModels.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Extensions.DependencyInjection;

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

        public List<ISeries> SaldosSeries { get; set; }
        public List<ISeries> SaldosPerStudentSeries { get; set; }
        public List<ISeries> StalinSeries { get; set; }
        public List<ISeries> CostPerStalinSeries { get; set; }

        public IRelayCommand OpenInfoPopupCommand { get;set; }

        private readonly ApplicationDataStore _applicationDataStore;
        private readonly IPopupService _popupService;
        public PlotterViewModel(ApplicationDataStore applicationDataStore, IServiceProvider serviceProvider, IPopupService popupService) : base(serviceProvider)
        {
            var saldos = new List<decimal>();
            var saldosPerStudent = new List<decimal>();
            var stalin = new List<decimal>();
            var costPerStalin = new List<decimal>();

            OpenInfoPopupCommand = new RelayCommand(OpenInfoPopup);

            for (int i = 0; i < applicationDataStore.Schools.Count; i++)
            {
                saldos.Add(applicationDataStore.Schools[i].Saldo());
                saldosPerStudent.Add(applicationDataStore.Schools[i].SaldoPerStudent());
                stalin.Add(applicationDataStore.Schools[i].GetTrzyStaliny());
                costPerStalin.Add(applicationDataStore.Schools[i].GetCostPerStanin());
            }

            var columnSeries1 = new ColumnSeries<decimal>
            {
                Values = saldos,
                Stroke = null,
                Padding = 2
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
