using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models;
using MusicSystem.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicSystem.Controllers
{
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService customerService;
        public CustomersController(CustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet("Customer")]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await customerService.GetAll());
        }

        [HttpGet("Customer/{customerId}")]
        public async Task<ActionResult<Customer>> GetById(int customerId)
        {
            var customer = await customerService.GetById(customerId);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost("Customer")]
        public async Task<IActionResult> Add(Customer customer)
        {
            int result = await customerService.Add(customer);
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { customerId = customer.CustomerId }, customer);

            }
            return BadRequest();
        }

        [HttpPut("Customer")]
        public async Task<IActionResult> Update(int customerId, Customer customer)
        {
            if (customerId != customer.CustomerId)
                return BadRequest();

            var existingCustomer = await customerService.GetById(customerId);

            if (existingCustomer == null)
                return NotFound();

            customerService.Update(customer);
            return NoContent();
        }

        [HttpDelete("Customer/{customerId}")]
        public async Task<IActionResult> Delete(int customerId)
        {
            var customer = await customerService.GetById(customerId);

            if (customer == null)
                return NotFound();

            customerService.Delete(customerId);

            return NoContent();
        }

        [HttpGet("Customer/PreviousPurchases/{customerId}")]
        public async Task<ActionResult> GetPrevious(int customerId)
        {
            return Ok(await customerService.GetPreviousPurchases(customerId));
        }

        //[HttpGet("GetAlbum/{albumId}")]
        //public ActionResult<List<TrackDTO>> GetAlbums(int albumId)
        //{
        //    return Ok(albumService.GetAlbumById(albumId));
        //}

        //// GET api/<ValuesController>/5
        //[HttpGet("GetAlbum/{albumId}")]
        //public ActionResult<List<Tracks>> GetAlbum(int albumId)
        //{
        //    var album = albumService.LoadListFromDB().Where(e => e.AlbumId == albumId).ToList();
        //    if (album == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(album);  

        //}


        //[HttpPost("AddAlbum")]
        //public ActionResult<List<Album>> AddAlbum(string title, int artistId)
        //{
        //    if (addAlbumService.Add(title, artistId) is not null) { return CreatedAtAction(nameof(GetAlbums), "New Record Inserted"); }
        //    return NotFound();
        //}

        //[HttpDelete("DeleteAlbum{albumId}")]
        //public ActionResult DeleteAlbum(int albumId) {
        //    return Ok(deleteAlbumService.DeleteAlbum(albumId)
        //    );
        //}

    }
}