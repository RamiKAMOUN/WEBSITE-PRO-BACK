
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
    public class NosTechnolgController : ControllerBase
    {

        private readonly DataContext context;
        private readonly DataContext _context;

        public NosTechnolgController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<NosTechnolg>>> GetAllNosTech()
        {
            var nosTechs = await _context.NosTechnolgs.ToListAsync();



            return Ok(nosTechs);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<NosTechnolg>> NosTech(int id)
        {
            var NosTec = await _context.NosTechnolgs.FindAsync(id);
            if (NosTec is null)
                return NotFound("Technologie not found.");

            return Ok(NosTec);
        }
        [HttpPost("add-Technologie-with-image")]
        [Consumes("multipart/form-data")] // Set the Content-Type to multipart/form-data
        public async Task<ActionResult<NosTechnolg>> AddTechWithImage([FromForm] NosTechnolgDto tecDto)
        {
            try
            {
                if (tecDto == null || tecDto.AvatarFile == null)
                    return BadRequest("Please provide tec data and an image file.");

                // Validate the image file
                var file = tecDto.AvatarFile;
                if (file.Length == 0 || !file.ContentType.StartsWith("image"))
                    return BadRequest("Please provide a valid image file.");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Create a new Temoignage object and populate its properties from the DTO
                    NosTechnolg newTec = new NosTechnolg
                    {
                        Name = tecDto.Name,

                        Avatar = memoryStream.ToArray() // Store the image data as byte[] in the 'Avatar' property
                    };

                    _context.NosTechnolgs.Add(newTec);
                    await _context.SaveChangesAsync();

                    return Ok(newTec);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut]

        public async Task<ActionResult<List<NosTechnolg>>> UpdateNosTech(NosTechnolg updatedNosTech)
        {
            var dbNosTech = await _context.NosTechnolgs.FindAsync(updatedNosTech.Id);
            if (dbNosTech is null)
                return NotFound("Tec not found.");

            dbNosTech.Name = updatedNosTech.Name;

            dbNosTech.Avatar = updatedNosTech.Avatar;

            await _context.SaveChangesAsync();

            return Ok(await _context.NosTechnolgs.ToListAsync());
        }

        [HttpDelete]

        public async Task<ActionResult<List<NosTechnolg>>> DeleteNosTech(int id)
        {
            var dbNosTech = await _context.NosTechnolgs.FindAsync(id);
            if (dbNosTech is null)
                return NotFound("NosTech not found.");

            _context.NosTechnolgs.Remove(dbNosTech);

            await _context.SaveChangesAsync();

            return Ok(await _context.NosTechnolgs.ToListAsync());
        }



    }
}

