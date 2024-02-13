
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
    public class PosteController : ControllerBase
    {

        private readonly DataContext context;
        private readonly DataContext _context;

        public PosteController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Poste>>> GetAllPoste()
        {
            var postes = await _context.Postes.ToListAsync();



            return Ok(postes);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Poste>> Poste(int id)
        {
            var Post = await _context.Postes.FindAsync(id);
            if (Post is null)
                return NotFound("Poste not found.");

            return Ok(Post);
        }
        [HttpPost("add-post-with-image")]
        [Consumes("multipart/form-data")] // Set the Content-Type to multipart/form-data
        public async Task<ActionResult<Poste>> AddPosteWithImage([FromForm] PosteDto posteDto)
        {
            try
            {
                if (posteDto == null || posteDto.AvatarFile == null)
                    return BadRequest("Please provide poste data and an image file.");

                // Validate the image file
                var file = posteDto.AvatarFile;
                if (file.Length == 0 || !file.ContentType.StartsWith("image"))
                    return BadRequest("Please provide a valid image file.");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Create a new Temoignage object and populate its properties from the DTO
                    Poste newPoste = new Poste
                    {
                        Name = posteDto.Name,
                        Description = posteDto.Description,

                        Avatar = memoryStream.ToArray() // Store the image data as byte[] in the 'Avatar' property
                    };

                    _context.Postes.Add(newPoste);
                    await _context.SaveChangesAsync();

                    return Ok(newPoste);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut]

        public async Task<ActionResult<List<Poste>>> UpdatePoste(Poste updatedPoste)
        {
            var dbPoste = await _context.Postes.FindAsync(updatedPoste.Id);
            if (dbPoste is null)
                return NotFound("Poste not found.");

            dbPoste.Name = updatedPoste.Name;
            dbPoste.Description = updatedPoste.Description;
            dbPoste.Avatar = updatedPoste.Avatar;

            await _context.SaveChangesAsync();

            return Ok(await _context.Postes.ToListAsync());
        }

        [HttpDelete]

        public async Task<ActionResult<List<Poste>>> DeletePoste(int id)
        {
            var dbPoste = await _context.Postes.FindAsync(id);
            if (dbPoste is null)
                return NotFound("Poste not found.");

            _context.Postes.Remove(dbPoste);

            await _context.SaveChangesAsync();

            return Ok(await _context.Postes.ToListAsync());
        }



    }
}
