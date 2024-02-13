
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
    public class MessageController : ControllerBase
    {

        private readonly DataContext context;
        private readonly DataContext _context;

        public MessageController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Message>>> GetAllMessage()
        {
            var messages = await _context.Messages.ToListAsync();



            return Ok(messages);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Message>> Message(int id)
        {
            var messg = await _context.Messages.FindAsync(id);
            if (messg is null)
                return NotFound("message not found.");

            return Ok(messg);
        }
        [HttpPost]

        public async Task<ActionResult<List<Message>>> AddMessage(Message messg)
        {
            _context.Messages.Add(messg);
            await _context.SaveChangesAsync();


            return Ok(await _context.Messages.ToListAsync());
        }

        [HttpPut]

        public async Task<ActionResult<List<Message>>> UpdateMessage(Message updatedMessage)
        {
            var dbMessage = await _context.Messages.FindAsync(updatedMessage.Id);
            if (dbMessage is null)
                return NotFound("Message not found.");

            dbMessage.Name = updatedMessage.Name;
            dbMessage.Email = updatedMessage.Email;
            dbMessage.Telephone = updatedMessage.Telephone;
            dbMessage.Sujet = updatedMessage.Sujet;
            dbMessage.Messag = updatedMessage.Messag;
            
            await _context.SaveChangesAsync();

            return Ok(await _context.Messages.ToListAsync());
        }

        [HttpDelete]

        public async Task<ActionResult<List<Message>>> DeleteMessage(int id)
        {
            var dbMessage = await _context.Messages.FindAsync(id);
            if (dbMessage is null)
                return NotFound("Message not found.");

            _context.Messages.Remove(dbMessage);

            await _context.SaveChangesAsync();

            return Ok(await _context.Messages.ToListAsync());
        }
    }
}

