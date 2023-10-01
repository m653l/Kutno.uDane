using Application.Stores;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public class MainViewModel : CoreViewModel
    {
        private ShellViewModel _shellViewModel;
        public ShellViewModel ShellViewModel
        {
            get => _shellViewModel;
            set
            {
                _shellViewModel = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CoreViewModel> _popups = new ObservableCollection<CoreViewModel>();
        public ObservableCollection<CoreViewModel> Popups
        {
            get { return _popups; }
            set
            {
                _popups = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAnyPopupOpen));
            }
        }

        public bool IsAnyPopupOpen => Popups.Any();


        private readonly NavigationStore _navigationStore;

        public CoreViewModel? CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainViewModel(IServiceProvider serviceProvider, ShellViewModel shellViewModel, NavigationStore navigationStore) : base(serviceProvider)
        {
            _navigationStore = navigationStore;
            _shellViewModel = shellViewModel;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            Popups.CollectionChanged += Popups_CollectionChanged;

            SwitchToDefaultView();
        }

        private void Popups_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsAnyPopupOpen));
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        internal void SwitchToDefaultView()
        {
            _navigation.NavigateTo<BoberViewModel>();
        }
    }
}