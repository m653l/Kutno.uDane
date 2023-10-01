using Application.Services.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace Application.ViewModels.Controls
{
    public class ErrorDialogViewModel : CoreViewModel
    {
        private readonly IPopupService _popupService;

        private string _title = "Informacja";
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public IRelayCommand ClosePopupCommand { get; set; }

        public ErrorDialogViewModel(IServiceProvider serviceProvider, IPopupService popupService) : base(serviceProvider)
        {
            _popupService = popupService;

            ClosePopupCommand = new RelayCommand(ClosePopup);
        }

        private void ClosePopup()
        {
            _popupService.ClosePopup(this);
        }
    }
}
