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
        public decimal Expenses { get; set; } = 0;
        public decimal CostPerStanin { get; set; } = 0;

        public decimal Saldo()
        {
            return SumOfMoney; // + Income - Expenses
        }

        public decimal SaldoPerStudent()
        {
            return Saldo()/StudentCount;
        }

        public int GetTrzyStaliny()
        {
            if(MathExam.Stanin == null || PolishExam.Stanin == null || EngExam.Stanin == null)
                return 0;

            int mathStalin = MathExam.Stanin.Value;
            int polishStalin = PolishExam.Stanin.Value;
            int engStalin = EngExam.Stanin.Value;

            return mathStalin + polishStalin + engStalin;
        }

        public decimal GetCostPerStanin()
        {
            decimal result = SaldoPerStudent() / GetTrzyStaliny() / 3;
            CostPerStanin = result;
            return result;
        }
    }
}
