using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintersManagerBackend.Models
{
    public class PrinterStatistic
    {
        public int Id { get; set; }
        public string State { get; set; }   
        public string Status { get; set; }
        public string PrintStatus { get; set; }
        public string WorkTime { get; set; }
        public string PagesNumber { get; set; }
        public int? PrinterId { get; set; }
        public ICollection<TonerStatistic>? TonerStatistic { get; set; }
        public Printer? Printer { get; set; }
    }
}
