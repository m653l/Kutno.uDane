namespace Domain.Aggregates
{
    public class School
    {
        public string Name { get; set; } = string.Empty;
        public decimal StudentCount { get; set; }
        public Exam MathExam { get; set; } = new();
        public Exam PolishExam { get; set; } = new();
        public Exam EngExam { get; set; } = new();

        public decimal AvgMoneyPerStudent { get; set; }
        public decimal SumOfMoney { get; set; }

        public decimal Income { get; set; } = 0;
        public decimal Expens { get; set; } = 0;
    }
}
