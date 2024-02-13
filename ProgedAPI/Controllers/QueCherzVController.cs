
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
    public class QueCherzVController : ControllerBase
    {

        private readonly DataContext context;
        private readonly DataContext _context;

        public QueCherzVController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<QueCherzV>>> GetAllQueCherzVs()
        {
            var queCherzVs = await _context.QueCherzVs.ToListAsync();



            return Ok(queCherzVs);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<QueCherzV>> HQueCherzV(int id)
        {
            var QeChrzV = await _context.QueCherzVs.FindAsync(id);
            if (QeChrzV is null)
                return NotFound("QueChercherVous not found.");

            return Ok(QeChrzV);
        }
        [HttpPost]

        public async Task<ActionResult<List<QueCherzV>>> AddQueCherzV(QueCherzV QeChrzV)
        {
            _context.QueCherzVs.Add(QeChrzV);
            await _context.SaveChangesAsync();


            return Ok(await _context.QueCherzVs.ToListAsync());
        }

        [HttpPut]

        public async Task<ActionResult<List<QueCherzV>>> UpdateQueCherzV(QueCherzV updatedQueCherzV)
        {
            var dbQueCherzV = await _context.QueCherzVs.FindAsync(updatedQueCherzV.Id);
            if (dbQueCherzV is null)
                return NotFound("QueCherzV not found.");

            dbQueCherzV.Title = updatedQueCherzV.Title;
            dbQueCherzV.desc = updatedQueCherzV.desc;

            await _context.SaveChangesAsync();

            return Ok(await _context.QueCherzVs.ToListAsync());
        }

        [HttpDelete]

        public async Task<ActionResult<List<QueCherzV>>> DeleteQueCherzV(int id)
        {
            var dbQueCherzV = await _context.QueCherzVs.FindAsync(id);
            if (dbQueCherzV is null)
                return NotFound("QueCherzV not found.");

            _context.QueCherzVs.Remove(dbQueCherzV);

            await _context.SaveChangesAsync();

            return Ok(await _context.QueCherzVs.ToListAsync());
        }
    }
}

