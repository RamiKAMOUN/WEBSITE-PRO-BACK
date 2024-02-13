
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
    public class CerviceController : ControllerBase
    {

        private readonly DataContext context;
        private readonly DataContext _context;

        public CerviceController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cervice>>> GetAllCervices()
        {
            var cervices = await _context.Cervices.ToListAsync();



            return Ok(cervices);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Cervice>> Cervice(int id)
        {
            var Cerv = await _context.Cervices.FindAsync(id);
            if (Cerv is null)
                return NotFound("Cervice not found.");

            return Ok(Cerv);
        }
        [HttpPost("add-cervice-with-image")]
        [Consumes("multipart/form-data")] // Set the Content-Type to multipart/form-data
        public async Task<ActionResult<Cervice>> AddCerviceWithImage([FromForm] CerviseDto cerviseDto)
        {
            try
            {
                if (cerviseDto == null || cerviseDto.AvatarFile == null)
                    return BadRequest("Please provide cervice data and an image file.");

                // Validate the image file
                var file = cerviseDto.AvatarFile;
                if (file.Length == 0 || !file.ContentType.StartsWith("image"))
                    return BadRequest("Please provide a valid image file.");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Create a new Temoignage object and populate its properties from the DTO
                    Cervice newCervice = new Cervice
                    {
                        Name = cerviseDto.Name,
                        Description = cerviseDto.Description,
                      
                        Avatar = memoryStream.ToArray() // Store the image data as byte[] in the 'Avatar' property
                    };

                    _context.Cervices.Add(newCervice);
                    await _context.SaveChangesAsync();

                    return Ok(newCervice);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut]

        public async Task<ActionResult<List<Cervice>>> UpdateCervice(Cervice updatedCervice)
        {
            var dbCervice = await _context.Cervices.FindAsync(updatedCervice.Id);
            if (dbCervice is null)
                return NotFound("Cervice not found.");

            dbCervice.Name = updatedCervice.Name;
            dbCervice.Description = updatedCervice.Description;
            dbCervice.Avatar = updatedCervice.Avatar;

            await _context.SaveChangesAsync();

            return Ok(await _context.Cervices.ToListAsync());
        }

        [HttpDelete]

        public async Task<ActionResult<List<Cervice>>> DeleteCervice(int id)
        {
            var dbCervice = await _context.Cervices.FindAsync(id);
            if (dbCervice is null)
                return NotFound("Cervice not found.");

            _context.Cervices.Remove(dbCervice);

            await _context.SaveChangesAsync();

            return Ok(await _context.Cervices.ToListAsync());
        }



    }
}
