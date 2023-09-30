using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public partial class ShellViewModel : CoreViewModel
    {
        public ShellViewModel(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [RelayCommand]
        public void GoToDashboard()
        {
            _navigation.NavigateTo<DashboardViewModel>();
        }
        [RelayCommand]
        public void GoToPlotter()
        {
            _navigation.NavigateTo<PlotterViewModel>();
        }
        [RelayCommand]
        public void GoToSummary()
        {
            _navigation.NavigateTo<SummaryViewModel>();
        }
        [RelayCommand]
        public void GoToSettings()
        {
            _navigation.NavigateTo<SettingsViewModel>();
        }
    }
}
