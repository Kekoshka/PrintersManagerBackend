namespace PrintersManagerBackend.Models
{
    public class TonerStatistic
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string Total { get; set; }
        public string Spent { get; set; }
        public int? PrinterStatisticId { get; set; }
        public PrinterStatistic? PrinterStatistic { get; set; }
    }
}
