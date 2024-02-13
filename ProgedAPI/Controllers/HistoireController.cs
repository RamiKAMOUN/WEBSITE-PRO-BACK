
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgedAPI.Data;
using ProgedAPI.Entities;
using System.Runtime.InteropServices;

namespace ProgedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoireController : ControllerBase
    {

        private readonly DataContext context;
        private readonly DataContext _context;

        public HistoireController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Histoire>>> GetAllHistoires()
        {
            var histoires = await _context.Histoires.ToListAsync();



            return Ok(histoires);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Histoire>> Histoire(int id)
        {
            var histo = await _context.Histoires.FindAsync(id);
            if (histo is null)
                return NotFound("histo not found.");

            return Ok(histo);
        }
        [HttpPost]

        public async Task<ActionResult<List<Histoire>>> AddHero(Histoire histo)
        {
            _context.Histoires.Add(histo);
            await _context.SaveChangesAsync();


            return Ok(await _context.Histoires.ToListAsync());
        }

        [HttpPut]

        public async Task<ActionResult<List<Histoire>>> UpdateHisto(Histoire updatedHisto)
        {
            var dbHisto = await _context.Histoires.FindAsync(updatedHisto.Id);
            if (dbHisto is null)
                return NotFound("Histo not found.");

            dbHisto.Title = updatedHisto.Title;
            dbHisto.desc = updatedHisto.desc;

            await _context.SaveChangesAsync();

            return Ok(await _context.Histoires.ToListAsync());
        }

        [HttpDelete]

        public async Task<ActionResult<List<Histoire>>> DeleteHisto(int id)
        {
            var dbHisto = await _context.Histoires.FindAsync(id);
            if (dbHisto is null)
                return NotFound("Histo not found.");

            _context.Histoires.Remove(dbHisto);

            await _context.SaveChangesAsync();

            return Ok(await _context.Histoires.ToListAsync());
        }
    }
}
