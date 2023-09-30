using Application.Services.Interfaces;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public partial class DashboardViewModel : CoreViewModel
    {
        public static List<FilePickerFileType> Types = new List<FilePickerFileType>();
        public static FilePickerFileType XmlType { get; } = new("All Images")
        {
            Patterns = new[] { "*.xml", "*.xlsx", "*.xlsm" }
        };

        [ObservableProperty]
        private string _sioFilePath = string.Empty;
        [ObservableProperty]
        private string _schoolsFilePath = string.Empty;
        [ObservableProperty]
        private string _expensesFilePath = string.Empty;
        [ObservableProperty]
        private string _incomesFilePath = string.Empty;

        private readonly IImportDataService _importDataService;
        public DashboardViewModel(IServiceProvider serviceProvider, IImportDataService importDataService) : base(serviceProvider)
        {
            Types.Add(XmlType);

            _importDataService = importDataService;
        }
        [RelayCommand]
        public async Task PickSio(Control view)
        {
            SioFilePath = await PickFileAsync(view);
        }
        [RelayCommand]
        public async Task PickSchools(Control view)
        {
            SchoolsFilePath = await PickFileAsync(view);
        }
        [RelayCommand]
        public async Task PickExpenses(Control view)
        {
            ExpensesFilePath = await PickFileAsync(view);
        }
        [RelayCommand]
        public async Task PickIncomes(Control view)
        {
            IncomesFilePath = await PickFileAsync(view);
        }
        [RelayCommand]
        public async Task ReadData()
        {
            _importDataService.ImportExamsData(SchoolsFilePath);
            _importDataService.ImportSioData(SioFilePath);
        }

        private async Task<string> PickFileAsync(Control view)
        {
            var topLevel = TopLevel.GetTopLevel(view);

            // Start async operation to open the dialog.
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                AllowMultiple = false,
                FileTypeFilter = Types
            });

            if (files.Count >= 1)
            {
                //// Open reading stream from the first file.
                //await using var stream = await files[0].OpenReadAsync();
                //using var streamReader = new StreamReader(stream);
                //// Reads all the content of file as a text.
                //var fileContent = await streamReader.ReadToEndAsync();

                return files[0].Path.ToString();
            }

            return string.Empty;
        }
    }
}
