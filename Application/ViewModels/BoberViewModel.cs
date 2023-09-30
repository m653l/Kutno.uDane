using Application.Commands;
using Application.Mappings;
using Application.Models;
using Application.Services.Interfaces;
using Application.ViewModels.Controls;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ViewModels
{
    public class BoberViewModel : CoreViewModel, IMapFrom<MainBeaverDTO>
    {
        public int Id { get; set; }

        private string? _name;
        public string? Name 
        { 
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public IRelayCommand GoNextCommand { get; set; }
        public IAsyncRelayCommand LoadDataCommand { get; set; }
        public IAsyncRelayCommand SaveBoberCommand { get; set; }
        public IRelayCommand OpenPopupCommand { get; set; }

        public BoberViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            GoNextCommand = new RelayCommand(() => _navigation.NavigateTo<PlotViewModel>());
            LoadDataCommand = new AsyncRelayCommand(SwitchToDefaultView);
            SaveBoberCommand = new AsyncRelayCommand(SaveBober);
            OpenPopupCommand = new RelayCommand(OpenPopup);
        }

        private async Task SaveBober()
        {
            var qwe = _serviceProvider.GetRequiredService<IImportDataService>();
        }

        private void OpenPopup()
        {
            var _popupService = _serviceProvider.GetRequiredService<IPopupService>();
            var vm = _serviceProvider.GetRequiredService<JumpScareDialogViewModel>();
            _popupService.OpenPopup(vm);
        }

        private async Task SwitchToDefaultView()
        {
            var vm = await _mediator.Send(new GetBeaverQuery());

            Name = vm.Name;
            Id = vm.Id;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MainBeaverDTO, BoberViewModel>()
                .ConstructUsing(x => _serviceProvider.GetRequiredService<BoberViewModel>())
                .ForMember(
                    vm => vm.SaveBoberCommand,
                    opt => opt.Ignore())
                .ForMember(
                    vm => vm.GoNextCommand,
                    opt => opt.Ignore())
                .ForMember(
                    vm => vm.LoadDataCommand,
                    opt => opt.Ignore());
        }
    }
}
