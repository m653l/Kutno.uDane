using CommunityToolkit.Mvvm.Input;

namespace Application.ViewModels
{
    public class WelcomeViewModel : CoreViewModel
    {
        private readonly ShellViewModel _shellViewModel;

        public IRelayCommand GoNextCommand { get; set; }

        public WelcomeViewModel(IServiceProvider serviceProvider, ShellViewModel shellViewModel) : base(serviceProvider)
        {
            _shellViewModel = shellViewModel;

            GoNextCommand = new RelayCommand(GoNext);
        }

        private void GoNext()
        {
            _shellViewModel.ActivateView();
            _navigation.NavigateTo<DashboardViewModel>();
        }
    }
}
