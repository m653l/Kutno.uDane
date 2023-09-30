using Application.Services.Interfaces;
using Application.ViewModels;
using Application.ViewModels.Helpers;

namespace AUI.Services
{
    public class PopupService : IPopupService
    {
        private readonly MainViewModel _mainViewModel;
        private readonly IUIContext _uiContext;

        public PopupService(MainViewModel mainViewModel, IUIContext uiContext)
        {
            _mainViewModel = mainViewModel;
            _uiContext = uiContext;
        }

        public void OpenPopup(CoreViewModel viewModel)
        {
            _uiContext.Invoke(() =>
            {
                _mainViewModel.Popups.Add(viewModel);
            });
        }
        public void ClosePopup(CoreViewModel viewModel)
        {
            _uiContext.Invoke(() =>
            {
                _mainViewModel.Popups.Remove(viewModel);
            });
        }
    }
}
