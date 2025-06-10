using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Microsoft.EntityFrameworkCore;
using PrintersManagerBackend.Context;
using PrintersManagerBackend.Interfaces;
using PrintersManagerBackend.Models;
using System.Net;

namespace PrintersManagerBackend.Services
{
    public class PrinterService : IPrinterService
    {

        List<Printer> PrintersList;
        ApplicationContext _context;
        public PrinterService(ApplicationContext context) 
        {
            _context = context;
        }
        public async Task CheckPrintersStatisticsAsync()
        {
            PrintersList = _context.Printers.Include(p => p.Model).ThenInclude(m => m.Toners).ToList();

            List<PrinterStatistic> updatedPrinterStatisticList = new();

            foreach(var printer in PrintersList)
            {
                try
                {
                    var printerStatistic = await _context.PrinterStatistics.Include(ps => ps.TonerStatistic).FirstOrDefaultAsync(ps => ps.PrinterId == printer.Id);
                
                    if(printerStatistic is null)
                    {
                        printerStatistic = new();
                        printerStatistic.TonerStatistic = new List<TonerStatistic>();
                    }
                    if(printerStatistic.TonerStatistic is not null)
                        printerStatistic.TonerStatistic.Clear();
                
                    var basicStatistic = await Messenger.GetAsync(VersionCode.V2,
                          new IPEndPoint(IPAddress.Parse(printer.IP), printer.Port),
                          new OctetString("public"),
                          new List<Variable> {
                                  new Variable(new ObjectIdentifier(printer.Model.State)),
                                  new Variable(new ObjectIdentifier(printer.Model.Status)),
                                  new Variable(new ObjectIdentifier(printer.Model.PrintStatus)),
                                  new Variable(new ObjectIdentifier(printer.Model.WorkTime)),
                                  new Variable(new ObjectIdentifier(printer.Model.PagesNumber)),
                          });

                    printerStatistic.State = basicStatistic[0].Data.ToString();
                    printerStatistic.Status = basicStatistic[1].Data.ToString();
                    printerStatistic.PrintStatus = basicStatistic[2].Data.ToString();
                    printerStatistic.WorkTime = basicStatistic[3].Data.ToString();
                    printerStatistic.PagesNumber = basicStatistic[4].Data.ToString();

                    foreach (var toner in printer.Model.Toners)
                    {
                        var tonerStatistic = await Messenger.GetAsync(VersionCode.V2,
                            new IPEndPoint(IPAddress.Parse(printer.IP), printer.Port),
                            new OctetString("public"),
                            new List<Variable> {
                                    new Variable(new ObjectIdentifier(toner.Total)),
                                    new Variable(new ObjectIdentifier(toner.Spent))
                            });

                        printerStatistic.TonerStatistic.Add(new TonerStatistic
                            {
                                Color = toner.Color,
                                Total = tonerStatistic[0].Data.ToString(),
                                Spent = tonerStatistic[1].Data.ToString(),
                            });
                    }
                    updatedPrinterStatisticList.Add(printerStatistic);
                }
                catch
                {

                }
            }

            
            _context.AddRange(updatedPrinterStatisticList);
            await _context.SaveChangesAsync();
        }
    }
}
