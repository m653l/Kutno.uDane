using Application.Services.Interfaces;
using Application.Stores;
using OfficeOpenXml;

namespace Application.Services
{
    public class ImportDataService : IImportDataService
    {
        private readonly ApplicationDataStore _applicationDataStore;

        public ImportDataService(ApplicationDataStore applicationDataStore)
        {
            _applicationDataStore = applicationDataStore;
        }

        public void ImportExamsData()
        {
            string filePath = "C:\\Users\\xarda\\Downloads\\Kutno_HackSQL\\Kutno_HackSQL\\Wyniki_e8_szkoly_2022.xlsx"; // Replace with the path to your .xlsx file

            // Set the LicenseContext to suppress the license exception
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming you want to read the first worksheet

                List<string> filteredData = new List<string>();

                // Start from the third row to ignore the first two rows
                for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
                {
                    string province = worksheet.Cells[row, 3].Text; // Assuming Column C is the third column (index 3)

                    if (province.Equals("Kutno", StringComparison.OrdinalIgnoreCase))
                    {
                        // Extract specific columns (I, and from L to Z)
                        string rowData = worksheet.Cells[row, 9].Text + "\t"; // Column I

                        for (int col = 12; col <= 26; col++) // Columns L to Z
                        {
                            rowData += worksheet.Cells[row, col].Text + "\t";
                        }

                        filteredData.Add(rowData.Trim()); // Remove trailing tab

                    }
                }

                List<string> parsedData = new List<string>();

                foreach (string item in filteredData)
                {
                    string replacedItem = item.Replace("\t", ",");
                    parsedData.Add(replacedItem);
                }

                _applicationDataStore.ExamsResult = filteredData;
            }
        }
    }
}
