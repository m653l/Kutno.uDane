using AUI.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Infrastructure;
using Application;
using Infrastructure.LocalDatabase.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using NLog.Extensions.Logging;

namespace AUI
{
    public partial class App : Avalonia.Application
    {
        public static new App Current => (App)Avalonia.Application.Current;

        private IHost _host;

        public IServiceProvider Services => _host.Services;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            IHostBuilder hostBuilder = new HostBuilder()
            .ConfigureHostConfiguration(config =>
            {
                config.AddEnvironmentVariables("ASPNETCORE_");
            })
            .ConfigureServices((context, services) =>
            {
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddNLog(context.Configuration);
                });
                services.AddInfrastructure()
                    .AddApplication()
                    .AddUI();

                if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
                    services.AddSingleton<MainView>();
            });

            _host = hostBuilder.Build();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                desktopLifetime.MainWindow = Services.GetRequiredService<MainWindow>();
                desktopLifetime.MainWindow.Show();

                desktopLifetime.Exit += async (sender, e) =>
                {
                    using (_host)
                    {
                        await _host.StopAsync();
                    }
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = Services.GetRequiredService<MainView>();
            }

            InitializeDatabase();

            base.OnFrameworkInitializationCompleted();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void InitializeDatabase()
        {
            using IServiceScope scope = Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetRequiredService<DataContext>().Database.EnsureCreated();
            ILogger<App> logger = Current.Services.GetRequiredService<ILogger<App>>();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ILogger<App> logger = Current.Services.GetRequiredService<ILogger<App>>();
            logger.LogError(e.ExceptionObject.ToString(), "Global unhandled exception");
        }
    }
}