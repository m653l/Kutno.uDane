using Application.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Application.ViewModels
{
    public partial class PlotterViewModel : CoreViewModel
    {
        public List<ISeries> SaldosSeries { get; set; }
        public List<ISeries> SaldosPerStudentSeries { get; set; }
        public List<ISeries> StalinSeries { get; set; }
        public List<ISeries> CostPerStalinSeries { get; set; }
        private readonly ApplicationDataStore _applicationDataStore;
        public PlotterViewModel(ApplicationDataStore applicationDataStore, IServiceProvider serviceProvider) : base(serviceProvider) 
        {
            var saldos = new List<decimal>();
            var saldosPerStudent = new List<decimal>();
            var stalin = new List<decimal>();
            var costPerStalin = new List<decimal>();

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
        }
    }
}
