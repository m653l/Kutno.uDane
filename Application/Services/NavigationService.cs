using Application.Services.Interfaces;
using Application.Stores;
using Application.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationStore _navigationStore;

        public NavigationService(IServiceProvider serviceProvider,
            NavigationStore navigationStore)
        {
            _serviceProvider = serviceProvider;
            _navigationStore = navigationStore;
        }

        public TViewModel NavigateTo<TViewModel>() where TViewModel : CoreViewModel
        {
            TViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            _navigationStore.CurrentViewModel = viewModel;
            return viewModel;
        }

        public TViewModel? NavigateTo<TViewModel>(Func<TViewModel?> factory) where TViewModel : CoreViewModel
        {
            TViewModel? viewModel = factory();
            AssertNotNull(viewModel);
            _navigationStore.CurrentViewModel = viewModel;
            return viewModel;
        }

        public async Task<TViewModel?> NavigateTo<TViewModel>(Func<Task<TViewModel?>> factory) where TViewModel : CoreViewModel
        {
            TViewModel? viewModel = await factory();
            AssertNotNull(viewModel);
            _navigationStore.CurrentViewModel = viewModel;
            return viewModel;
        }

        private static void AssertNotNull<TViewModel>(TViewModel? viewModel) where TViewModel : CoreViewModel
            => _ = viewModel ?? throw new InvalidOperationException(
                $"Setting current view model to null is forbidden for type {typeof(TViewModel).Name}");
    }
}
