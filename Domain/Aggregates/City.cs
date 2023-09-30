namespace Domain.Aggregates
{
    public class City
    {
        public string Name { get; set; } = string.Empty;
        public List<School> Schools { get; set; } = new();
        public int PolishStanin { get; set; }
        public int MathStanin { get; set; }
        public int EngStanin { get; set; }

        private int CalculateCityStanin()
        {
            return 1;
        }
    }
}
