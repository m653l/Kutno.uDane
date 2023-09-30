namespace Application.Services.Interfaces
{
    public interface IImportDataService
    {
        void ImportExamsData(string filePath);
        void ImportSioData(string filePath);
    }
}
