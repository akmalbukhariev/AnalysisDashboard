namespace AnalysisDashboard.Models
{
    public class BarChartItemInfo
    {
        public string Date { get; set; }
        public string Sum { get; set; }
        public string PurposeOfPayment { get; set; }

        public void Copy(BarChartItemInfo other)
        {
            Date = other.Date;
            Sum = other.Sum;
            PurposeOfPayment = other.PurposeOfPayment;
        }
    }
}
