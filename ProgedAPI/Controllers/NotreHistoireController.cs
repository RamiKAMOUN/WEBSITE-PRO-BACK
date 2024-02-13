
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
    public class NotreHistoireController : ControllerBase
    {

        private readonly DataContext context;
        private readonly DataContext _context;

        public NotreHistoireController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<NotreHistoire>>> GetAllNotreHistoires()
        {
            var notreHistoires = await _context.NotreHistoires.ToListAsync();



            return Ok(notreHistoires);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<NotreHistoire>> NotreHistoire(int id)
        {
            var NotreHis = await _context.NotreHistoires.FindAsync(id);
            if (NotreHis is null)
                return NotFound("notreHistoire not found.");

            return Ok(NotreHis);
        }
        [HttpPost]

        public async Task<ActionResult<List<NotreHistoire>>> AddNotreHistoire(NotreHistoire NotreHis)
        {
            _context.NotreHistoires.Add(NotreHis);
            await _context.SaveChangesAsync();


            return Ok(await _context.NotreHistoires.ToListAsync());
        }

        [HttpPut]

        public async Task<ActionResult<List<NotreHistoire>>> UpdateNotreHistoire(NotreHistoire updatedNotreHistoire)
        {
            var dbNotreHis = await _context.NotreHistoires.FindAsync(updatedNotreHistoire.Id);
            if (dbNotreHis is null)
                return NotFound("NotreHistoire not found.");

            dbNotreHis.Title = updatedNotreHistoire.Title;
            dbNotreHis.IconBg = updatedNotreHistoire.IconBg;
            dbNotreHis.Date = updatedNotreHistoire.Date;
            dbNotreHis.Points = updatedNotreHistoire.Points;

            await _context.SaveChangesAsync();

            return Ok(await _context.NotreHistoires.ToListAsync());
        }

        [HttpDelete]

        public async Task<ActionResult<List<NotreHistoire>>> DeleteNotreHistoire(int id)
        {
            var dbNotreHis = await _context.NotreHistoires.FindAsync(id);
            if (dbNotreHis is null)
                return NotFound("NotreHistoire not found.");

            _context.NotreHistoires.Remove(dbNotreHis);

            await _context.SaveChangesAsync();

            return Ok(await _context.NotreHistoires.ToListAsync());
        }
    }
}
