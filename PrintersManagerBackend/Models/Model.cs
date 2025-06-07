using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintersManagerBackend.Models
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public string PrintStatus { get; set; }
        public string WorkTime { get; set; }
        public string PagesNumber { get; set; }
        public ICollection<Toner>? Toners { get; set; }
        public ICollection<Printer>? Printers { get; set; }
    }
}
