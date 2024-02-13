
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
    public class ClientController : ControllerBase
    {

        private readonly DataContext context;
        private readonly DataContext _context;

        public ClientController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> GetAllClient()
        {
            var clients = await _context.Clients.ToListAsync();



            return Ok(clients);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Client>> Client(int id)
        {
            var Cli = await _context.Clients.FindAsync(id);
            if (Cli is null)
                return NotFound("Client not found.");

            return Ok(Cli);
        }
        [HttpPost("add-client-with-image")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Client>> AddClientWithImage([FromForm] ClientDto clientDto)
        {
            try
            {
                if (clientDto == null || clientDto.AvatarFile == null)
                    return BadRequest("Please provide Client data and an image file.");

                // Validate the image file
                var file = clientDto.AvatarFile;
                if (file.Length == 0 || !file.ContentType.StartsWith("image"))
                    return BadRequest("Please provide a valid image file.");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Create a new Temoignage object and populate its properties from the DTO
                    Client newClient = new Client
                    {
                        Name = clientDto.Name,
                     

                        Avatar = memoryStream.ToArray() // Store the image data as byte[] in the 'Avatar' property
                    };

                    _context.Clients.Add(newClient);
                    await _context.SaveChangesAsync();

                    return Ok(newClient);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut]

        public async Task<ActionResult<List<Client>>> UpdateClient(Client updatedClient)
        {
            var dbClient = await _context.Clients.FindAsync(updatedClient.Id);
            if (dbClient is null)
                return NotFound("Client not found.");

            dbClient.Name = updatedClient.Name;
           
            dbClient.Avatar = updatedClient.Avatar;

            await _context.SaveChangesAsync();

            return Ok(await _context.Clients.ToListAsync());
        }

        [HttpDelete]

        public async Task<ActionResult<List<Client>>> DeleteClient(int id)
        {
            var dbClient = await _context.Clients.FindAsync(id);
            if (dbClient is null)
                return NotFound("Client not found.");

            _context.Clients.Remove(dbClient);

            await _context.SaveChangesAsync();

            return Ok(await _context.Clients.ToListAsync());
        }



    }
}
