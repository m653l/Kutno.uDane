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
        public void GoToDashboardCommand()
        {
            _navigation.NavigateTo<DashboardViewModel>();
        }
        [RelayCommand]
        public void GoToPlotterCommand()
        {
            _navigation.NavigateTo<PlotterViewModel>();
        }
        [RelayCommand]
        public void GoToSummaryCommand()
        {
            _navigation.NavigateTo<SummaryViewModel>();
        }
        [RelayCommand]
        public void GoToSettingsCommand()
        {
            _navigation.NavigateTo<SettingsViewModel>();
        }
    }
}
