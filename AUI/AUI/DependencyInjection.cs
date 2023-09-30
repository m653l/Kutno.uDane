using Application.Services.Interfaces;
using Application.ViewModels.Helpers;
using AUI.Services;
using AUI.Views;
using AUI.Views.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AUI
{
    public static class DependencyInjection
    {
        public static void AddUI(this IServiceCollection services)
        {
            _ = services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            _ = services.AddSingleton<IUIContext, AvaloniaContext>();
            _ = services.AddSingleton<IPopupService, PopupService>();

            _ = services.AddSingleton<MainWindow>();
            _ = services.AddSingleton<MainView>();
        }
    }
}
