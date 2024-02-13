
using Microsoft.AspNetCore.Cors;
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
    
    public class CertificationController : ControllerBase
    {

        private readonly DataContext context;
        private readonly DataContext _context;

        public CertificationController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Certification>>> GetAllCertification()
        {
            var certifications = await _context.Certifications.ToListAsync();



            return Ok(certifications);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Certification>> Certification(int id)
        {
            var Certif = await _context.Certifications.FindAsync(id);
            if (Certif is null)
                return NotFound(" Certification not found.");

            return Ok(Certif);
        }
        [HttpPost("add-certification-with-image")]
        [Consumes("multipart/form-data")] // Set the Content-Type to multipart/form-data
        public async Task<ActionResult<Certification>> AddCertificationWithImage([FromForm] CertificationDto certifDto)
        {
            try
            {
                if (certifDto == null || certifDto.AvatarFile == null)
                    return BadRequest("Please provide Certif image .");

                // Validate the image file
                var file = certifDto.AvatarFile;
                if (file.Length == 0 || !file.ContentType.StartsWith("image"))
                    return BadRequest("Please provide a valid image file.");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Create a new Familly object and populate its properties from the DTO
                    Certification newCertification = new Certification
                    {
                        Avatar = memoryStream.ToArray() // Store the image data as byte[] in the 'Avatar' property
                    };

                    _context.Certifications.Add(newCertification);
                    await _context.SaveChangesAsync();

                    return Ok(newCertification);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut]

        public async Task<ActionResult<List<Certification>>> UpdateFamille(Certification updatedCertif)
        {
            var dbCertif = await _context.Certifications.FindAsync(updatedCertif.Id);
            if (dbCertif is null)
                return NotFound("Certification Pic not found.");

            dbCertif.Avatar = updatedCertif.Avatar;

            await _context.SaveChangesAsync();

            return Ok(await _context.Certifications.ToListAsync());
        }

        [HttpDelete]

        public async Task<ActionResult<List<Certification>>> DeleteCertification(int id)
        {
            var dbCertif = await _context.Certifications.FindAsync(id);
            if (dbCertif is null)
                return NotFound("Certification Pic not found.");

            _context.Certifications.Remove(dbCertif);

            await _context.SaveChangesAsync();

            return Ok(await _context.Certifications.ToListAsync());
        }



    }
}
