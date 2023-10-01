using Application.Models;
using Application.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.ConditionalDraw;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Themes;
using SkiaSharp;

namespace Application.ViewModels
{
    public partial class PlotterViewModel : CoreViewModel
    {
        public List<ISeries> SaldosSeries { get; set; }
        public List<ISeries> SaldosPerStudentSeries { get; set; }
        public List<ISeries> StalinSeries { get; set; }
        public List<ISeries> CostPerStalinSeries { get; set; }

        SolidColorPaint[] _paints;

        [ObservableProperty]
        private Axis[] _xAxes = { new Axis { IsVisible = false } };
        [ObservableProperty]
        private Axis[] _yAxes = { new Axis { IsVisible = false, } };

        private readonly ApplicationDataStore _applicationDataStore;
        public PlotterViewModel(ApplicationDataStore applicationDataStore, IServiceProvider serviceProvider) : base(serviceProvider) 
        {
            _applicationDataStore = applicationDataStore;

            XAxes[0].Labels = _applicationDataStore.ActiveYear.Schools.Select(i => i.Name).ToList();

            var saldos = new List<PilotInfo>();
            var saldosPerStudent = new List<PilotInfo>();
            var stalin = new List<PilotInfo>();
            var costPerStalin = new List<PilotInfo>();

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
        }
    }
}
