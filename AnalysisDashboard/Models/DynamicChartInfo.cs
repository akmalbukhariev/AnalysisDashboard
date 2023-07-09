namespace AnalysisDashboard.Models
{
    public class DynamicChartInfo
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public double Amount { get; set; }

        public void Copy(DynamicChartInfo other)
        {
            Year = other.Year;
            Month = other.Month;    
            Day = other.Day;
            Amount = other.Amount;
        }
    }
}
