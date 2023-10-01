using CommunityToolkit.Mvvm.Input;

namespace Application.ViewModels
{
    public partial class ShellViewModel : CoreViewModel
    {
        public ShellViewModel(IServiceProvider serviceProvider) : base(serviceProvider) { }

        private bool _isViewVisable = false;
        public bool IsViewVisable
        {
            get => _isViewVisable;
            set
            {
                _isViewVisable = value;
                OnPropertyChanged();
            }
        }

        private bool _plotterActivator = false;
        public bool PlotterActivator
        {
            get => _plotterActivator;
            set
            {
                _plotterActivator = value;
                OnPropertyChanged();
            }
        }

        private bool _summaryActivator = false;
        public bool SummaryActivator
        {
            get => _summaryActivator;
            set
            {
                _summaryActivator = value;
                OnPropertyChanged();
            }
        }

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

        public void ActivateButtons()
        {
            PlotterActivator = true; 
            SummaryActivator = true;
        }

        public void ActivateView()
        {
            IsViewVisable = true;
        }
    }
}
