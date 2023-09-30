using Application.Mappings;
using Application.Services;
using Application.Services.Interfaces;
using Application.Stores;
using Application.ViewModels;
using Application.ViewModels.Controls;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile(provider));
                IEnumerable<BaseMappingProfile> profiles = provider.GetServices<BaseMappingProfile>();
                cfg.AddProfiles(profiles);
            }).CreateMapper());

            _ = services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            _ = services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("pl-PL");

            AddViewModels(services);

            services.AddTransient<INavigationService, NavigationService>();

            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ApplicationDataStore>();

            return services;
        }

        private static void AddViewModels(IServiceCollection services)
        {
            services.AddSingleton<ShellViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<PlotViewModel>();
            services.AddTransient<BoberViewModel>();
            services.AddTransient<KapibaraViewModel>();

            services.AddTransient<JumpScareDialogViewModel>();
        }
    }
}
