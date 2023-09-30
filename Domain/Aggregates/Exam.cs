using Domain.Enums;

namespace Domain.Aggregates
{
    public class Exam
    {
        public ExamType ExamType { get; set; } 
        public int Attendees { get; set; }
        public decimal? Avg { get; set; }
        public decimal? Deviation { get; set; }
        public decimal? Median { get; set; }
        public decimal? Modal { get; set; }
    }
}
