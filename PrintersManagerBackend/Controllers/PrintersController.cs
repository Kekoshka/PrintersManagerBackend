using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintersManagerBackend.Context;
using PrintersManagerBackend.Models;

namespace PrintersManagerBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintersController : ControllerBase
    {
        ApplicationContext _context;
        public PrintersController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var printers = await _context.Printers.Include(p => p.PrinterStatistic).ThenInclude(ps => ps.TonerStatistic).ToListAsync();

            var DTOPrinters = printers.Select(p => new Printer
            {
                Id = p.Id,
                IP = p.IP,
                Name = p.Name,
                Port = p.Port,
                PrinterStatistic = p.PrinterStatistic is null ? null : new PrinterStatistic
                {
                    Id = p.PrinterStatistic.Id,
                    State = p.PrinterStatistic.State,
                    Status = p.PrinterStatistic.Status,
                    PrintStatus = p.PrinterStatistic.PrintStatus,
                    PagesNumber = p.PrinterStatistic.PagesNumber,
                    WorkTime = p.PrinterStatistic.WorkTime,
                    TonerStatistic = p.PrinterStatistic.TonerStatistic is null ? null : p.PrinterStatistic.TonerStatistic.Select(ts => new TonerStatistic
                    {
                        Color = ts.Color,
                        Total = ts.Total,
                        Spent = ts.Spent
                    }).ToList()
                }
            });

            return Ok(DTOPrinters);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Printer printer)
        {
            Printer newPrinter = new()
            {
                IP = printer.IP,
                Port = printer.Port,
                ModelId = printer.ModelId,
                Name = printer.Name,
            };
            await _context.Printers.AddAsync(newPrinter);
            await _context.SaveChangesAsync();
            return Ok(newPrinter);
        }
        [HttpPut]
        public async Task<IActionResult> Put(Printer printer)
        {
            var updatedPrinter = await _context.Printers.Include(p => p.PrinterStatistic).FirstOrDefaultAsync(p => p.Id == printer.Id);
            if (updatedPrinter is null)
                return NotFound();

            updatedPrinter.ModelId = printer.ModelId;
            updatedPrinter.IP = printer.IP;
            updatedPrinter.Name = printer.Name;

            if (updatedPrinter.PrinterStatistic is not null)
                _context.Remove(updatedPrinter.PrinterStatistic);

            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int printerId)
        {
            var deletedPrinter = await _context.Printers.FindAsync(printerId);
            if (deletedPrinter is null)
                return NotFound();

            _context.Remove(deletedPrinter);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
