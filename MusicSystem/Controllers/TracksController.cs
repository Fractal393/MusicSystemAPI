using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models;
using MusicSystem.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicSystem.Controllers
{
    public class TracksController : ControllerBase
    {
        private readonly TrackService trackService;
        public TracksController(TrackService trackService)
        {
            this.trackService = trackService;
        }

        [HttpGet("Track")]
        public async Task<ActionResult<Track>> GetAll()
        {
            var result = await trackService.GetAll();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("Track/{trackId}")]
        public async Task<ActionResult<Track>> GetById(int trackId)
        {
            var track = await trackService.GetById(trackId);

            if (track == null)
                return NotFound();

            return Ok(track);
        }

        [HttpPost("Track")]
        public async Task<IActionResult> Add(Track track)
        {
            int result = await trackService.Add(track);
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { trackId = track.TrackId }, track);

            }
            return BadRequest();
        }

        [HttpPut("Track")]
        public async Task<IActionResult> Update(int trackId, Track track)
        {
            if (trackId != track.TrackId)
                return BadRequest();

            var existingTrack = await trackService.GetById(trackId);

            if (existingTrack == null)
                return NotFound();

            trackService.Update(track);
            return NoContent();
        }

        [HttpDelete("Track/{trackId}")]
        public async Task<IActionResult> Delete(int trackId)
        {
            var track = await trackService.GetById(trackId);

            if (track == null)
                return NotFound();

            trackService.Delete(trackId);

            return NoContent();
        }

        [HttpGet("Track/Name/{trackName}")]

        public async Task<ActionResult<List<Track>>> GetTrackByName(string trackName)
        {
            var result = await trackService.GetTrackByName(trackName);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("Track/Playlist/{playlistId}")]

        public async Task<ActionResult> GetByPlaylistId(int playlistId)
        {
            //var playlist = customerService.LoadListFromDB().Where(e => e.CustomerId== customerId).ToList();
            //if (playlist == null)
            //{
            //    return NotFound(); 
            //}
            var result = await trackService.GetByPlaylistId(playlistId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("Track/Filter")]
        public async Task<ActionResult> GetByFilter(int? albumId, int? artistId, int? genreId)
        {
            var result = await trackService.GetByFilter(albumId, artistId, genreId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

    }
}
