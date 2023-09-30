using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface INavigationService
    {
        TViewModel NavigateTo<TViewModel>() where TViewModel : CoreViewModel;
        TViewModel? NavigateTo<TViewModel>(Func<TViewModel?> factory) where TViewModel : CoreViewModel;
        Task<TViewModel?> NavigateTo<TViewModel>(Func<Task<TViewModel?>> factory) where TViewModel : CoreViewModel;
    }
}
