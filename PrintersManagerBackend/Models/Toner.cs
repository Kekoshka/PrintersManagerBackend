using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintersManagerBackend.Models
{
    public class Toner
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string Total { get; set; }
        public string Spent { get; set; }
        public int? ModelId { get; set; }
        public Model? Model { get; set; }

    }
}
