using Application.Models;
using Application.Services.Interfaces;
using Application.Stores;
using Application.ViewModels.Controls;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.ConditionalDraw;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Themes;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Reflection.Emit;

namespace Application.ViewModels
{
    public partial class DashboardViewModel : CoreViewModel
    {
        public ObservableCollection<YearViewModel> Years
        {
            get
            {
                return _applicationDataStore.Years;
            }
        }

        public IRelayCommand AnalyzeDataCommand { get; set; }

        private readonly IImportDataService _importDataService;
        private readonly IPopupService _popupService;
        private readonly ApplicationDataStore _applicationDataStore;
        private readonly ShellViewModel _shellViewModel;
        private readonly IServiceProvider _serviceProvider;
        public DashboardViewModel(ApplicationDataStore applicationDataStore, IServiceProvider serviceProvider, IImportDataService importDataService, IPopupService popupService, ShellViewModel shellViewModel) : base(serviceProvider)
        {
            AnalyzeDataCommand = new RelayCommand(AnalyzeData);

            _applicationDataStore = applicationDataStore;
            _importDataService = importDataService;
            _popupService = popupService;
            _serviceProvider = serviceProvider;
            _shellViewModel = shellViewModel;
        }

        [RelayCommand]
        public void AddNewYearCommand()
        {
            Years.Add(new YearViewModel(_applicationDataStore, _serviceProvider, _importDataService, _popupService, _shellViewModel));
        }

        private void AnalyzeData()
        {
            foreach (YearViewModel? year in Years)
            {
                if (year is null) 
                { 
                    continue; 
                }
                else
                {
                    year.ReadData();
                    _applicationDataStore.ActiveYear = _applicationDataStore.Years.FirstOrDefault();
                }
            }
        }
    }
}
