using Application.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public partial class PlotterViewModel : CoreViewModel
    {
        public List<ISeries> SaldosSeries { get; set; }
        private readonly ApplicationDataStore _applicationDataStore;
        public PlotterViewModel(ApplicationDataStore applicationDataStore, IServiceProvider serviceProvider) : base(serviceProvider) 
        {
            var saldos = new List<decimal>();

            for (int i = 0; i < applicationDataStore.Schools.Count; i++)
            {
                saldos.Add(applicationDataStore.Schools[i].Saldo());
            }

            var columnSeries1 = new ColumnSeries<decimal>
            {
                Values = saldos,
                Stroke = null,
                Padding = 2
            };

            //var columnSeries2 = new ColumnSeries<decimal>
            //{
            //    Values = values2,
            //    Stroke = null,
            //    Padding = 2
            //};

            //var columnSeries3 = new ColumnSeries<decimal>
            //{
            //    Values = values3,
            //    Stroke = null,
            //    Padding = 2
            //};

            SaldosSeries = new List<ISeries> { columnSeries1 };
        }
    }
}
