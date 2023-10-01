using Application.ViewModels.Controls;

namespace Application.Services.Interfaces
{
    public interface IImportDataService
    {
        void ImportData(YearViewModel year, string sioPath, string examPath, string expenses, string incomesPath);
    }
}
