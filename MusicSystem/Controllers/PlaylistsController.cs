using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models;
using MusicSystem.Services;

namespace MusicSystem.Controllers
{
    public class PlaylistsController : ControllerBase
    {
        private readonly PlaylistService playlistService;

        public PlaylistsController(PlaylistService playlistService)
        {
            this.playlistService = playlistService;
        }

        [HttpGet("Playlist")]
        public async Task<ActionResult<PlaylistTrack>> GetAll()
        {
            return Ok(await playlistService.GetAll());
        }

        [HttpGet("Playlist/{playlistId}")]
        public async Task<ActionResult<PlaylistTrack>> GetById(int playlistId)
        {
            var playlistTrack = await playlistService.GetById(playlistId);

            if (playlistTrack == null)
                return NotFound();

            return Ok(playlistTrack);
        }

        [HttpPost("Playlist")]
        public async Task<IActionResult> Add(PlaylistTrack playlistTrack)
        {
            int result = await playlistService.Add(playlistTrack);
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { playlistId = playlistTrack.PlaylistId }, playlistTrack);

            }
            return BadRequest();

        }

        [HttpPut("Playlist")]
        public async Task<IActionResult> Update(int playlistId, PlaylistTrack playlistTrack)
        {
            if (playlistId != playlistTrack.PlaylistId)
                return BadRequest();

            var existingAlbum = await playlistService.GetById(playlistId);

            if (existingAlbum == null)
                return NotFound();

            playlistService.Update(playlistTrack);
            return NoContent();
        }

        [HttpDelete("Playlist/{playlistId}")]
        public async Task<IActionResult> Delete(int playlistId)
        {
            var playlistTrack = await playlistService.GetById(playlistId);

            if (playlistTrack == null)
                return NotFound();

            playlistService.Delete(playlistId);

            return NoContent();
        }
    }
}