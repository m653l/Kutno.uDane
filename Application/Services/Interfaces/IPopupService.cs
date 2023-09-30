using Application.ViewModels;

namespace Application.Services.Interfaces
{
    public interface IPopupService
    {
        void OpenPopup(CoreViewModel viewModel);
        void ClosePopup(CoreViewModel viewModel);
    }
}
