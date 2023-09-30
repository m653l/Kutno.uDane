using Application.Services.Interfaces;
using Application.Stores;
using OfficeOpenXml;

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

        private Dictionary<string, decimal> weights = new Dictionary<string, decimal>
        {
            { "P1", 0.40M },
            { "P2", 0.20M },
            { "P3", 0.33M },
            { "P4", 0.20M },
            { "P5", 1.40M },
            { "P6", 2.90M },
            { "P7", 3.60M },
            { "P8", 9.50M },
            { "P9", 0.80M },
            { "P10", 0.082M },

            { "P11", 0.12M },
            { "P12", 0.40M },
            { "P13", 0.95M },
            { "P14", 0.85M },
            { "P15", 0.23M },
            { "P16", 0.40M },
            { "P17", 0.35M },
            { "P18", 0.29M },
            { "P19", 0.23M },
            { "P20", 0.10M },

            { "P21", 0.08M },
            { "P22", 0.20M },
            { "P23", 0.08M },
            { "P24", 0.20M },
            { "P25", 0.08M },
            { "P26", 0.05M },
            { "P27", 0.20M },
            { "P27a", 0.10M },
            { "P28", 1.00M },
            { "P28a", 0.50M },
            { "P29", 1.30M },
            { "P29a", 0.65M },
            { "P30", 0.60M },

            { "P31", 0.20M },
            { "P32", 1.00M },
            { "P33", 0.85M },
            { "P34", 0.85M },
            { "P35", 1.50M },
            { "P36", 2.01M },
            { "P37", 3.36M },
            { "P38", 0.92M },
            { "P39", 1.00M },
            { "P40", 1.35M },

            { "P41", 1.10M },
            { "P42", 1.94M },
            { "P43", 0.60M },
            { "P44", 0.17M },
            { "P45", 0.06M },
            { "P46", 0.05M },
            { "P47", 3.00M },
            { "P48", 0.36M },
            { "P49", 0.345M },
            { "P50", 0.68M },

            { "P51", 0.065M },
            { "P52", 1.50M },
            { "P53", 0.40M },
            { "P54", 0.025M },
            { "P55", 0.012M },
            { "P56", 0.18M },
            { "P57", 0.112M },
            { "P58", 0.045M },
            { "P59", 0.015M },
            { "P60", 0.011M },

            { "P61", 0.75M },
            { "P62", 0.66M },
            { "P63", 0.15M },
            { "P64", 3.04M },
            { "P65", 1.50M },
            { "P66", 0.50M },
            { "P67", 3.64M },
            { "P68", 6.30M },
            { "P69", 6.50M },
            { "P70", 7.80M },

            { "P71", 5.00M },
            { "P72", 10.00M },
            { "P73", 1.50M },
            { "P74", 9.50M },
            { "P75", 0.02M },
            { "P76", 0.84M },
            { "P77", 0.25M },
            { "P78", 2.90M },
            { "P79", 3.60M },
            { "P80", 0.18M },

            { "P81", 0.112M },
            { "P82", 0.045M },
            { "P83", 0.015M },
            { "P84", 0.011M },
            { "P85", 0.191M },
            { "P86", 0.146M },
            { "P87", 0.079M },
            { "P88", 0.045M },
            { "P89", 0.039M },
            { "P90", 0.18M },
        };

        private void ImportSioData()
        {
            string filePath = "C:\\Users\\xarda\\Downloads\\Kutno_HackSQL\\Kutno_HackSQL\\SIO 30.09.2022.xlsx"; // Replace with the path to your .xlsx file

            // Set the LicenseContext to suppress the license exception
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming you want to read the first worksheet

                List<string> filteredData = new List<string>();

                for (int row = 7; row <= worksheet.Dimension.End.Row; row++)
                {
                    string establishmentType = worksheet.Cells[row, 11].Text;

                    if (establishmentType.Equals("Szkoła podstawowa", StringComparison.OrdinalIgnoreCase))
                    {
                        decimal avgPerStudent = 0;

                        for (int col = 38; col <= 132; col++) // Columns AL to DY
                        {
                            decimal cellValue = decimal.Parse(worksheet.Cells[row, col].Text);
                            //rowData += string.IsNullOrEmpty(cellValue) ? "null," : cellValue + ",";
                        }

                        //filteredData.Add(rowData.Trim()); // Remove trailing tab

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
    }
}
