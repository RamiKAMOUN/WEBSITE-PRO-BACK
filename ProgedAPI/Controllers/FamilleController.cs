
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
    public class FamilleController : ControllerBase
    {

        private readonly DataContext context;
        private readonly DataContext _context;

        public FamilleController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Famille>>> GetAllFamille()
        {
            var familles = await _context.Familles.ToListAsync();



            return Ok(familles);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Famille>> Famille(int id)
        {
            var Famil = await _context.Familles.FindAsync(id);
            if (Famil is null)
                return NotFound(" Photos not found.");

            return Ok(Famil);
        }
        [HttpPost("add-famille-with-image")]
        [Consumes("multipart/form-data")] // Set the Content-Type to multipart/form-data
        public async Task<ActionResult<Famille>> AddFamilleWithImage([FromForm] FamilleDto familleDto)
        {
            try
            {
                if (familleDto == null || familleDto.AvatarFile == null)
                    return BadRequest("Please provide image .");

                // Validate the image file
                var file = familleDto.AvatarFile;
                if (file.Length == 0 || !file.ContentType.StartsWith("image"))
                    return BadRequest("Please provide a valid image file.");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Create a new Familly object and populate its properties from the DTO
                    Famille newFamille = new Famille
                    {
                       Avatar = memoryStream.ToArray() // Store the image data as byte[] in the 'Avatar' property
                    };

                    _context.Familles.Add(newFamille);
                    await _context.SaveChangesAsync();

                    return Ok(newFamille);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut]

        public async Task<ActionResult<List<Famille>>> UpdateFamille(Famille updatedFamil)
        {
            var dbFamil = await _context.Familles.FindAsync(updatedFamil.Id);
            if (dbFamil is null)
                return NotFound("FamilPic not found.");

              dbFamil.Avatar = updatedFamil.Avatar;

            await _context.SaveChangesAsync();

            return Ok(await _context.Familles.ToListAsync());
        }

        [HttpDelete]

        public async Task<ActionResult<List<Famille>>> DeleteFamille(int id)
        {
            var dbFamil = await _context.Familles.FindAsync(id);
            if (dbFamil is null)
                return NotFound("FamilPic not found.");

            _context.Familles.Remove(dbFamil);

            await _context.SaveChangesAsync();

            return Ok(await _context.Familles.ToListAsync());
        }



    }
}
