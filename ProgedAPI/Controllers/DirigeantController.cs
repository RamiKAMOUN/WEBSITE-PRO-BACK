
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
    public class DirigeantController : ControllerBase
    {

        private readonly DataContext context;
        private readonly DataContext _context;

        public DirigeantController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Dirigeant>>> GetAllDirigeant()
        {
            var dirigeants = await _context.Dirigeants.ToListAsync();



            return Ok(dirigeants);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Dirigeant>> Dirigeant(int id)
        {
            var Dirig = await _context.Dirigeants.FindAsync(id);
            if (Dirig is null)
                return NotFound("Dirigeant not found.");

            return Ok(Dirig);
        }
        [HttpPost("add-dirigeant-with-image")]
        [Consumes("multipart/form-data")] // Set the Content-Type to multipart/form-data
        public async Task<ActionResult<Dirigeant>> AddDirigeantWithImage([FromForm] DirigeantDto dirigeantDto)
        {
            try
            {
                if (dirigeantDto == null || dirigeantDto.AvatarFile == null)
                    return BadRequest("Please provide dirigeant data and an image file.");

                // Validate the image file
                var file = dirigeantDto.AvatarFile;
                if (file.Length == 0 || !file.ContentType.StartsWith("image"))
                    return BadRequest("Please provide a valid image file.");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Create a new Temoignage object and populate its properties from the DTO
                    Dirigeant newDirigeant = new Dirigeant
                    {
                        Name = dirigeantDto.Name,
                        Description = dirigeantDto.Description,

                        Avatar = memoryStream.ToArray() // Store the image data as byte[] in the 'Avatar' property
                    };

                    _context.Dirigeants.Add(newDirigeant);
                    await _context.SaveChangesAsync();

                    return Ok(newDirigeant);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut]

        public async Task<ActionResult<List<Dirigeant>>> UpdateDirigeant(Dirigeant updatedDirigeant)
        {
            var dbDirigeant = await _context.Dirigeants.FindAsync(updatedDirigeant.Id);
            if (dbDirigeant is null)
                return NotFound("Dirigeant not found.");

            dbDirigeant.Name = updatedDirigeant.Name;
            dbDirigeant.Description = updatedDirigeant.Description;
            dbDirigeant.Avatar = updatedDirigeant.Avatar;

            await _context.SaveChangesAsync();

            return Ok(await _context.Dirigeants.ToListAsync());
        }

        [HttpDelete]

        public async Task<ActionResult<List<Dirigeant>>> DeleteDirigeant(int id)
        {
            var dbDirigeant = await _context.Dirigeants.FindAsync(id);
            if (dbDirigeant is null)
                return NotFound("Dirigeant not found.");

            _context.Dirigeants.Remove(dbDirigeant);

            await _context.SaveChangesAsync();

            return Ok(await _context.Dirigeants.ToListAsync());
        }



    }
}
