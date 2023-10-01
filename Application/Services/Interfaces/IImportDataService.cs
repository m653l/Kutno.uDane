namespace Application.Services.Interfaces
{
    public interface IImportDataService
    {
        void ImportData(string sioPath, string examPath, string expenses, string incomesPath);
    }
}
