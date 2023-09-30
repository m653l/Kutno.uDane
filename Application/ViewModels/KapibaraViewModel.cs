using CommunityToolkit.Mvvm.Input;

namespace Application.ViewModels
{
    public class KapibaraViewModel : CoreViewModel
    {
        public IRelayCommand GoBackCommand { get; set; }

        public KapibaraViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            GoBackCommand = new RelayCommand(() => _navigation.NavigateTo<BoberViewModel>());
        }
    }
}
