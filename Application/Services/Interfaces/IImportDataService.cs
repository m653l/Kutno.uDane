namespace Application.Services.Interfaces
{
    public interface IImportDataService
    {
        void ImportExamsData(string filePath);
        void ImportSioData(string filePath);
        void ImportIncome(string filePath);
        void ImportExpenses(string filePath);
    }
}
