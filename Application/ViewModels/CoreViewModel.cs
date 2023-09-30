using Application.Services.Interfaces;
using Application.ViewModels.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ViewModels
{

    public class CoreViewModel : ObservableObject
    {
        protected readonly IMediator _mediator;
        protected readonly IUIContext _uiContext;
        protected readonly INavigationService _navigation;
        protected readonly IServiceProvider _serviceProvider;

        public CoreViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mediator = serviceProvider.GetRequiredService<IMediator>();
            _uiContext = _serviceProvider.GetRequiredService<IUIContext>();
            _navigation = _serviceProvider.GetRequiredService<INavigationService>();
        }
    }
}