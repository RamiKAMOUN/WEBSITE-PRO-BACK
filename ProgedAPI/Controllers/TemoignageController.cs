

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgedAPI.Data;
using ProgedAPI.Entities;
using ProgedAPI.Migrations;
using System.Runtime.InteropServices;

namespace ProgedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemoignageController : ControllerBase
    {

        private readonly DataContext context;
        private readonly DataContext _context;

        public TemoignageController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Temoignage>>> GetAllTemoignage()
        {
            var temoignages = await _context.Temoignages.ToListAsync();



            return Ok(temoignages);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Temoignage>> Temoignage(int id)
        {
            var Temoig = await _context.Temoignages.FindAsync(id);
            if (Temoig is null)
                return NotFound("Temoignage not found.");

            return Ok(Temoig);
        }
        [HttpPost("add-Temoignage-with-image")]
        [Consumes("multipart/form-data")] // Set the Content-Type to multipart/form-data
        public async Task<ActionResult<Temoignage>> AddTemoignageWithImage([FromForm] TemoignageWithImageDto temDto)
        {
            try
            {
                if (temDto == null || temDto.AvatarFile == null)
                    return BadRequest("Please provide Temoignage data and an image file.");

                // Validate the image file
                var file = temDto.AvatarFile;
                if (file.Length == 0 || !file.ContentType.StartsWith("image"))
                    return BadRequest("Please provide a valid image file.");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Create a new Temoignage object and populate its properties from the DTO
                    Temoignage newTemoignage = new Temoignage
                    {
                        Name = temDto.Name,
                        Job = temDto.Job,
                        Quote=temDto.Quote,

                        Avatar = memoryStream.ToArray() // Store the image data as byte[] in the 'Avatar' property
                    };

                    _context.Temoignages.Add(newTemoignage);
                    await _context.SaveChangesAsync();

                    return Ok(newTemoignage);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut]

        public async Task<ActionResult<List<Temoignage>>> UpdateTemoignage(Temoignage updatedTemoignage)
        {
            var dbTemoignage = await _context.Temoignages.FindAsync(updatedTemoignage.Id);
            if (dbTemoignage is null)
                return NotFound("Temoignage not found.");

            dbTemoignage.Name = updatedTemoignage.Name;
            dbTemoignage.Job = updatedTemoignage.Job;
            dbTemoignage.Quote = updatedTemoignage.Quote;
            dbTemoignage.Avatar = updatedTemoignage.Avatar;

            await _context.SaveChangesAsync();

            return Ok(await _context.Temoignages.ToListAsync());
        }

        [HttpDelete]

        public async Task<ActionResult<List<Temoignage>>> DeleteTemoignage(int id)
        {
            var dbTemoignage = await _context.Temoignages.FindAsync(id);
            if (dbTemoignage is null)
                return NotFound("Temoignage not found.");

            _context.Temoignages.Remove(dbTemoignage);

            await _context.SaveChangesAsync();

            return Ok(await _context.Temoignages.ToListAsync());
        }



    }
}
