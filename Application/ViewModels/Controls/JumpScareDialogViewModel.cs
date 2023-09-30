using Application.Services.Interfaces;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ViewModels.Controls
{
    public class JumpScareDialogViewModel : CoreViewModel
    {
        public IRelayCommand ClosePopupCommand { get; set; }

        public JumpScareDialogViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ClosePopupCommand = new RelayCommand(ClosePopup);   
        }

        private void ClosePopup()
        {
            IPopupService popupService = _serviceProvider.GetRequiredService<IPopupService>();
            popupService.ClosePopup(this);
        }
    }
}
