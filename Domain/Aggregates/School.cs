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
        public decimal SumOfMoney { get; set; } = 0;
        public decimal Income { get; set; } = 0;
        public decimal Expenses { get; set; } = 0;

        public decimal Saldo { get; set; } = 0;
        public decimal SaldoPerStudent { get; set; } = 0;
        public int TrzyStaliny { get; set; } = 0;
        public decimal CostPerStanin { get; set; } = 0;

        public decimal GetSaldo()
        {
            Saldo = SumOfMoney;
            return (SumOfMoney + Income - Expenses) * -1;
        }

        public decimal GetSaldoPerStudent()
        {
            var result = GetSaldo() / StudentCount;
            SaldoPerStudent = result;
            return result;
        }

        public int GetTrzyStaliny()
        {
            if(MathExam.Stanin == null || PolishExam.Stanin == null || EngExam.Stanin == null)
                return 0;

            int mathStalin = MathExam.Stanin.Value;
            int polishStalin = PolishExam.Stanin.Value;
            int engStalin = EngExam.Stanin.Value;

            int result = mathStalin + polishStalin + engStalin;
            TrzyStaliny = result;
            return result;
        }

        public decimal GetCostPerStanin()
        {
            decimal result = GetSaldoPerStudent() / GetTrzyStaliny() / 3;
            CostPerStanin = result;
            return result;
        }
    }
}
