using Microsoft.EntityFrameworkCore;
using PrintersManagerBackend.Models;

namespace PrintersManagerBackend.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Models.Model> Models { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<PrinterStatistic> PrinterStatistics { get; set; }
        public DbSet<Toner> Toners { get; set; }
        public DbSet<TonerStatistic> TonerStatistics { get; set; }
        public ApplicationContext(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        } 
    }
}
