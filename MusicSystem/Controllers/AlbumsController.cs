using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models;
using MusicSystem.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicSystem.Controllers
{

    public class AlbumsController : ControllerBase
    {
        private readonly AlbumService albumService;

        public AlbumsController(AlbumService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet("Album")]
        public async Task<ActionResult<Album>> GetAll()
        {
            var result = await albumService.GetAll();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("Album/{albumId}")]
        public async Task<ActionResult<Album>> GetById(int albumId)
        {
            var album = albumService.GetById(albumId);

            if (album == null)
                return NotFound();

            return Ok(await album);
        }

        [HttpPost("Album")]
        public async Task<IActionResult> Add(Album album)
        {
            var result = await albumService.Add(album);
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { albumId = album.AlbumId }, album);
            }
            return BadRequest();
        }

        [HttpPut("Album")]
        public async Task<IActionResult> Update(int albumId, Album album)
        {
            if (albumId != album.AlbumId)
                return BadRequest();

            var existingAlbum = await albumService.GetById(albumId);

            if (existingAlbum == null)
                return NotFound();

            albumService.Update(album);
            return NoContent();
        }

        [HttpDelete("Album/{albumId}")]
        public async Task<IActionResult> Delete(int albumId)
        {
            var album = await albumService.GetById(albumId);

            if (album == null)
                return NotFound();

            albumService.Delete(albumId);

            return NoContent();
        }
    }
}
