using Application.Services.Interfaces;
using Application.Stores;
using Domain.Dictionaries;
using OfficeOpenXml;
using System.Xml;

namespace Application.Services
{
    public class ImportDataService : IImportDataService
    {
        private readonly ApplicationDataStore _applicationDataStore;
        private readonly decimal moneyPerStudent = 6081.3219M;

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
                    string province = worksheet.Cells[row, 10].Text; // Assuming Column C is the third column (index 3)

                    if (province.Equals("Kutno", StringComparison.OrdinalIgnoreCase))
                    {
                        // Extract specific columns (I, and from L to Z)
                        string rowData = worksheet.Cells[row, 9].Text + ","; // Column I

                        for (int col = 12; col <= 26; col++) // Columns L to Z
                        {
                            string cellValue = worksheet.Cells[row, col].Text;
                            rowData += string.IsNullOrEmpty(cellValue) ? "null," : cellValue + ",";
                        }

                        filteredData.Add(rowData.Trim()); // Remove trailing tab

                    }
                }

                foreach (string item in filteredData)
                {
                    string[] values = item.Split(',');

                    _applicationDataStore.Schools.Add(new Domain.Aggregates.School
                    {
                        Name = values[0],
                        StudentCount = 0,
                        PolishExam = new Domain.Aggregates.Exam
                        {
                            ExamType = Domain.Enums.ExamType.Polish,
                            Attendees = int.Parse(values[1]),
                            Avg = values[2] == "null" ? (int?)null : int.Parse(values[2]),
                            Deviation = values[3] == "null" ? (int?)null : int.Parse(values[3]),
                            Median = values[4] == "null" ? (int?)null : int.Parse(values[4]),
                            Modal = values[5] == "null" ? (int?)null : int.Parse(values[5])
                        },
                        MathExam = new Domain.Aggregates.Exam
                        {
                            ExamType = Domain.Enums.ExamType.Polish,
                            Attendees = int.Parse(values[6]),
                            Avg = values[7] == "null" ? (int?)null : int.Parse(values[7]),
                            Deviation = values[8] == "null" ? (int?)null : int.Parse(values[8]),
                            Median = values[9] == "null" ? (int?)null : int.Parse(values[9]),
                            Modal = values[10] == "null" ? (int?)null : int.Parse(values[10])
                        },
                        EngExam = new Domain.Aggregates.Exam
                        {
                            ExamType = Domain.Enums.ExamType.Polish,
                            Attendees = int.Parse(values[11]),
                            Avg = values[12] == "null" ? (int?)null : int.Parse(values[12]),
                            Deviation = values[13] == "null" ? (int?)null : int.Parse(values[13]),
                            Median = values[14] == "null" ? (int?)null : int.Parse(values[14]),
                            Modal = values[15] == "null" ? (int?)null : int.Parse(values[15])
                        }
                    });
                }
            }
        }

        public void ImportSioData()
        {
            //string filePath = "C:\\Users\\xarda\\Downloads\\Kutno_HackSQL\\Kutno_HackSQL\\SIO30.09.2022.xml"; // Replace with the path to your .xlsx file

            //// Set the LicenseContext to suppress the license exception
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //using (var package = new ExcelPackage(new FileInfo(filePath)))
            //{
            //    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming you want to read the first worksheet

            //    List<string> filteredData = new List<string>();

            //    for (int row = 7; row <= worksheet.Dimension.End.Row; row++)
            //    {
            //        string establishmentType = worksheet.Cells[row, 11].Text;

            //        if (establishmentType.Equals("Szkoła podstawowa", StringComparison.OrdinalIgnoreCase))
            //        {
            //            decimal avgNumerator = 0;
            //            decimal studentCount = Math.Round(decimal.Parse(worksheet.Cells[row, 34].Text));

            //            // Update missing student count
            //            _applicationDataStore.Schools.FirstOrDefault(x => x.Name == worksheet.Cells[row, 12].Text).StudentCount = studentCount;

            //            for (int col = 38; col <= 132; col++) // Columns AL to DY
            //            {
            //                string header = worksheet.Cells[6, col].Text;

            //                decimal weight = WeightsDictionaries.GetWeight(header);

            //                decimal pCount = decimal.Parse(worksheet.Cells[row, col].Text);

            //                avgNumerator += (pCount * weight * moneyPerStudent);
            //            }

            //            decimal avgPerStudent = avgNumerator / studentCount;

            //            decimal schoolSum = avgNumerator;
            //        }
            //    }
            //}

            string filePath = "C:\\Users\\xarda\\Downloads\\Kutno_HackSQL\\Kutno_HackSQL\\SIO30.09.2022.xml"; // Replace with the path to your XML file

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                // Assuming your XML structure has nodes representing schools or establishments
                XmlNodeList schoolNodes = xmlDoc.SelectNodes("//SchoolNode"); // Replace with the appropriate XPath query

                foreach (XmlNode schoolNode in schoolNodes)
                {
                    string establishmentType = schoolNode.SelectSingleNode("EstablishmentType").InnerText;

                    if (establishmentType.Equals("Szkoła podstawowa", StringComparison.OrdinalIgnoreCase))
                    {
                        decimal avgNumerator = 0;
                        decimal studentCount = Math.Round(decimal.Parse(schoolNode.SelectSingleNode("StudentCount").InnerText));

                        // Update missing student count (You need to implement _applicationDataStore.Schools)
                        // _applicationDataStore.Schools.FirstOrDefault(x => x.Name == schoolNode.SelectSingleNode("Name").InnerText).StudentCount = studentCount;

                        XmlNodeList dataNodes = schoolNode.SelectNodes("Data/Value"); // Assuming your data is stored within Value nodes

                        foreach (XmlNode dataNode in dataNodes)
                        {
                            string header = dataNode.SelectSingleNode("Header").InnerText;

                            decimal weight = WeightsDictionaries.GetWeight(header);

                            decimal pCount = decimal.Parse(dataNode.SelectSingleNode("Value").InnerText);

                            avgNumerator += (pCount * weight * moneyPerStudent);
                        }

                        decimal avgPerStudent = avgNumerator / studentCount;

                        decimal schoolSum = avgNumerator;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
