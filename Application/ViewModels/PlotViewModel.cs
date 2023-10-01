using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.Themes;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class PlotViewModel : CoreViewModel
    {
        private ISeries[] _series;
        public ISeries[] Series 
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ObservableValue> SeriesValues { get; } = new ObservableCollection<ObservableValue>
        {
            new ObservableValue(2),
            new ObservableValue(1),
            new ObservableValue(3),
            new ObservableValue(4),
            new ObservableValue(2),
        };

        public IRelayCommand GoBackCommand { get; set; }
        public IRelayCommand AddValueCommand { get; set; }

        public PlotViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Series = new ISeries[]
            {
                new LineSeries<ObservableValue>
                {
                    Values = SeriesValues,
                    Fill = null
                }
            };

            GoBackCommand = new RelayCommand(() => _navigation.NavigateTo<BoberViewModel>());
            AddValueCommand = new RelayCommand(AddValue);
        }

        private void AddValue()
        {
            SeriesValues.Add(new ObservableValue(2));
        }

        public LabelVisual Title { get; set; } =
            new LabelVisual
            {
                Text = "My chart title",
                TextSize = 25,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };
    }
}
