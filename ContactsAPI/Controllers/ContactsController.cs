using Microsoft.AspNetCore.Mvc;
using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ContactsController : Controller
    {
        private readonly ContactAPIDbContext dbContext;

        public ContactsController(ContactAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
           
        }

        [HttpPost]

        [HttpGet]
        [Route("{id:guid}")]

        public async Task<IActionResult> GetContact([FromRoute]Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }
        public async Task<IActionResult> AddContact(AddContact addContacts)
        {
            var contact = new Contact()
            {
                 Id = Guid.NewGuid(),
                Address = addContacts.Address,
                FullName = addContacts.FullName,
                Email = addContacts.Email,
                Phone = addContacts.Phone

            };
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            
            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute]Guid id,UpdateContact updatecontact)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact!= null)
            {
                contact.Email = updatecontact.Email;
                contact.Phone = updatecontact.Phone;
                contact.Address = updatecontact.Address;
                contact.FullName = updatecontact.FullName;

                await dbContext.SaveChangesAsync();
                return Ok(contact);

            }
            else
            {
                return NotFound();
            }


        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteContact([FromRoute]Guid id)
        {
            var contact = dbContext.Contacts.Find(id);
            if(contact == null)
            {
                return NotFound();
            }
            dbContext.Remove(contact);
            await dbContext.SaveChangesAsync();

            return Ok(contact);
        }
    }


}
