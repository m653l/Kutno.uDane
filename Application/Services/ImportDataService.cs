using Application.Services.Interfaces;
using Application.Stores;
using Application.ViewModels.Controls;
using Domain.Aggregates;
using Domain.Dictionaries;
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

        private void ImportExamsData(YearViewModel year, string filePath)
        {
            (int min, int max)[] StaninBordersPolish = CalculateStanin(filePath, 12, 13);
            (int min, int max)[] StaninBordersMath = CalculateStanin(filePath, 17, 18);
            (int min, int max)[] StaninBordersEng = CalculateStanin(filePath, 22, 23);

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

                    year.Schools.Add(new Domain.Aggregates.School
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
                            Modal = values[5] == "null" ? (int?)null : int.Parse(values[5]),
                            Stanin = GetStanin(StaninBordersPolish, values[2] == "null" ? (int?)null : int.Parse(values[2]))
                        },
                        MathExam = new Domain.Aggregates.Exam
                        {
                            ExamType = Domain.Enums.ExamType.Polish,
                            Attendees = int.Parse(values[6]),
                            Avg = values[7] == "null" ? (int?)null : int.Parse(values[7]),
                            Deviation = values[8] == "null" ? (int?)null : int.Parse(values[8]),
                            Median = values[9] == "null" ? (int?)null : int.Parse(values[9]),
                            Modal = values[10] == "null" ? (int?)null : int.Parse(values[10]),
                            Stanin = GetStanin(StaninBordersMath, values[7] == "null" ? (int?)null : int.Parse(values[7]))
                        },
                        EngExam = new Domain.Aggregates.Exam
                        {
                            ExamType = Domain.Enums.ExamType.Polish,
                            Attendees = int.Parse(values[11]),
                            Avg = values[12] == "null" ? (int?)null : int.Parse(values[12]),
                            Deviation = values[13] == "null" ? (int?)null : int.Parse(values[13]),
                            Median = values[14] == "null" ? (int?)null : int.Parse(values[14]),
                            Modal = values[15] == "null" ? (int?)null : int.Parse(values[15]),
                            Stanin = GetStanin(StaninBordersEng, values[12] == "null" ? (int?)null : int.Parse(values[12]))
                        }
                    });
                }
            }
        }

        private void ImportSioData(YearViewModel year, string filePath)
        {
            // Set the LicenseContext to suppress the license exception
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming you want to read the first worksheet

                for (int row = 7; row <= worksheet.Dimension.End.Row; row++)
                {
                    string establishmentType = worksheet.Cells[row, 11].Text;

                    if (establishmentType.Equals("Szkoła podstawowa", StringComparison.OrdinalIgnoreCase))
                    {
                        decimal avgNumerator = 0;
                        decimal studentCount = Math.Round(decimal.Parse(worksheet.Cells[row, 34].Text));

                        // Update missing student count
                        School school = year.Schools.FirstOrDefault(x => x.Name == worksheet.Cells[row, 12].Text);
                        school.StudentCount = studentCount;

                        for (int col = 38; col <= 129; col++) // Columns AL to DY
                        {
                            string header = worksheet.Cells[6, col].Text;

                            decimal weight = WeightsDictionaries.GetWeight(header);

                            decimal pCount = decimal.Parse(worksheet.Cells[row, col].Text);

                            avgNumerator += (pCount * weight * moneyPerStudent);
                        }

                        decimal avgPerStudent = avgNumerator / studentCount;

                        decimal schoolSum = avgNumerator + studentCount * moneyPerStudent;

                        school.AvgMoneyPerStudent = avgPerStudent;
                        school.SumOfMoney = schoolSum;
                    }
                }
            }
        }

        private void ImportIncome(YearViewModel year, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming you want to read the first worksheet

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    string searchText = worksheet.Cells[row, 17].Text.ToUpper();
                    School? school = string.IsNullOrEmpty(searchText)
                        ? null
                        : year.Schools.FirstOrDefault(x => x.Name.Contains(searchText));

                    if (school is null || worksheet.Cells[row, 33].Text == "")
                        continue;

                    school.Income += (decimal.Parse(worksheet.Cells[row, 33].Text) * 4);
                }
            }
        }

        private void ImportExpenses(YearViewModel year, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming you want to read the first worksheet

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    string searchText = worksheet.Cells[row, 17].Text.ToUpper();
                    School? school = string.IsNullOrEmpty(searchText)
                        ? null
                        : year.Schools.FirstOrDefault(x => x.Name.Contains(searchText));

                    if (school is null || worksheet.Cells[row, 35].Text == "")
                        continue;

                    school.Expenses += (decimal.Parse(worksheet.Cells[row, 35].Text) * 4);
                }
            }

            year.Schools.ToString();
        }

        private int? GetStanin((int min, int max)[] baseStanin, decimal? avg)
        {
            if (avg is null)
                return null;

            for (int i = 0; i <= baseStanin.Length; i++)
            {
                if (avg <= baseStanin[i].max && avg >= baseStanin[i].min)
                    return i + 1;
                else
                    continue;
            }

            return 1;
        }

        private (int min, int max)[] CalculateStanin(string filePath, int colIndexCount, int colIndexAvg)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming you want to read the first worksheet

                List<int> contestantCount = new List<int>();
                List<int> avgScorePolish = new List<int>();

                // Start from the third row to ignore the first two rows
                for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
                {
                    string vovoidship = worksheet.Cells[row, 1].Text;

                    if (vovoidship.Equals("Łódzkie", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(worksheet.Cells[row, colIndexCount].Text) || string.IsNullOrEmpty(worksheet.Cells[row, colIndexAvg].Text))
                            continue;

                        if (int.Parse(worksheet.Cells[row, colIndexCount].Text) < 5)
                            continue;

                        int rowData = int.Parse(worksheet.Cells[row, colIndexCount].Text); // Column I
                        contestantCount.Add(rowData);

                        int avgData = int.Parse(worksheet.Cells[row, colIndexAvg].Text);
                        avgScorePolish.Add(avgData);
                    }
                }

                int schoolsCount = contestantCount.Count;

                (int min, int max)[] sus = new(int, int)[9];

                // Stanin = 1 (< 4%)
                int stalin1Capacity = (int)Math.Round(schoolsCount * 0.04M);

                var sortedList = avgScorePolish.OrderBy(x => x).ToArray();

                sus[0] = (sortedList[0], sortedList[stalin1Capacity]);

                // Stain = 2 (4% - 10%)
                int stanin2Capacity = (int)Math.Round(schoolsCount * 0.1M);

                if (sortedList[stalin1Capacity + 1] == sortedList[stalin1Capacity])
                    sus[1] = (sortedList[stalin1Capacity + 1] + 1, sortedList[stanin2Capacity]);
                else
                    sus[1] = (sortedList[stalin1Capacity + 1], sortedList[stanin2Capacity]);

                // Stanin = 3 (11% - 22%)
                int stanin3Cap = (int)Math.Round(schoolsCount * 0.22M);

                if (sortedList[stanin2Capacity + 1] == sortedList[stanin2Capacity])
                    sus[2] = (sortedList[stanin2Capacity + 1] + 1, sortedList[stanin3Cap]);
                else
                    sus[2] = (sortedList[stanin2Capacity + 1], sortedList[stanin3Cap]);

                // Stanin = 4 (23% - 39%)
                int stanin4Cap = (int)Math.Round(schoolsCount * 0.39M);

                if (sortedList[stanin3Cap + 1] == sortedList[stanin3Cap])
                    sus[3] = (sortedList[stanin3Cap + 1] + 1, sortedList[stanin4Cap]);
                else
                    sus[3] = (sortedList[stanin3Cap + 1], sortedList[stanin4Cap]);

                // Stanin = 5 (40% - 59%)
                int stanin5Cap = (int)Math.Round(schoolsCount * 0.59M);

                if (sortedList[stanin4Cap + 1] == sortedList[stanin4Cap])
                    sus[4] = (sortedList[stanin4Cap + 1] + 1, sortedList[stanin5Cap]);
                else
                    sus[4] = (sortedList[stanin4Cap + 1], sortedList[stanin5Cap]);

                // Stanin = 6 (60% - 76%)
                int stanin6Cap = (int)Math.Round(schoolsCount * 0.76M);

                if (sortedList[stanin5Cap + 1] == sortedList[stanin5Cap])
                    sus[5] = (sortedList[stanin5Cap + 1] + 1, sortedList[stanin6Cap]);
                else
                    sus[5] = (sortedList[stanin5Cap + 1], sortedList[stanin6Cap]);

                // Stanin = 7 (77% - 88%)
                int stanin7Cap = (int)Math.Round(schoolsCount * 0.88M);

                if (sortedList[stanin6Cap + 1] == sortedList[stanin6Cap])
                    sus[6] = (sortedList[stanin6Cap + 1] + 1, sortedList[stanin7Cap]);
                else
                    sus[6] = (sortedList[stanin6Cap + 1], sortedList[stanin7Cap]);

                // Stanin = 8 (89% - 95%)
                int stanin8Cap = (int)Math.Round(schoolsCount * 0.95M);

                if (sortedList[stanin7Cap + 1] == sortedList[stanin7Cap])
                    sus[7] = (sortedList[stanin7Cap + 1] + 1, sortedList[stanin8Cap]);
                else
                    sus[7] = (sortedList[stanin7Cap + 1], sortedList[stanin8Cap]);

                // Stanin = 9 (> 95%)
                if (sortedList[stanin8Cap + 1] == sortedList[stanin8Cap])
                    sus[8] = (sortedList[stanin8Cap + 1] + 1, sortedList[sortedList.Count() - 1]);
                else
                    sus[8] = (sortedList[stanin8Cap + 1], sortedList[sortedList.Count() - 1]);

                return sus;
            }
        }

        public void ImportData(YearViewModel year, string sioPath, string examPath, string expenses, string incomesPath)
        {
            ImportExamsData(year, examPath);
            ImportSioData(year, sioPath);
            ImportIncome(year, incomesPath);
            ImportExpenses(year, expenses);
        }
    }
}
