using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintersManagerBackend.Models
{
    public class Printer
    {
        public int Id { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public int? ModelId { get; set; }
        public Model? Model { get; set; }
        public PrinterStatistic? PrinterStatistic { get; set; }
    }
}
