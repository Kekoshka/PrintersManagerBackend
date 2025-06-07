using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintersManagerBackend.Context;
using PrintersManagerBackend.Models;

namespace PrintersManagerBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase
    {
        ApplicationContext _context;
        public ModelsController(ApplicationContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var models = await _context.Models.Include(m => m.Toners).ToListAsync();
            if(models is null)
                return NotFound();
            return Ok(models);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Model model)
        {
            var newModel = new Model
            {
                PagesNumber = model.PagesNumber,
                PrintStatus = model.PrintStatus,
                State = model.State,
                Status = model.Status,
                WorkTime = model.WorkTime,
            };
            _context.Models.Add(newModel);
            await _context.SaveChangesAsync();
            foreach (var toner in model.Toners)
                newModel.Toners.Add(new Toner
                {
                    Color = toner.Color,
                    Total = toner.Total,
                    Spent = toner.Spent
                });
            await _context.SaveChangesAsync();
            return Ok(model);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Model model)
        {
            var updatedModel = await _context.Models.Include(m => m.Toners).FirstOrDefaultAsync(m => m.Id == model.Id);
            if (updatedModel is null)
                return NotFound();
            updatedModel.State = model.State;
            updatedModel.WorkTime = model.WorkTime;
            updatedModel.Status = model.Status;
            updatedModel.PagesNumber = model.PagesNumber;
            updatedModel.PrintStatus = model.PrintStatus;
            if(model.Toners is not null)
                foreach(var toner in model.Toners)
                {
                    var updatedToner = updatedModel.Toners.FirstOrDefault(t => t.Id == toner.Id);
                    if (updatedToner is null)
                        break;
                    updatedToner.Color = toner.Color;
                    updatedToner.Total = toner.Total;
                    updatedToner.Spent = toner.Spent;
                }
            return Ok(updatedModel);
        }
    }
}
