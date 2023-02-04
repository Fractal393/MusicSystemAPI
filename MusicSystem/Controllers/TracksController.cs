using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models;
using MusicSystem.Services;
using static MusicSystem.Services.PlaylistService;

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

        [HttpGet("Track/GetAll")]
        public async Task<ActionResult<Track>> GetAll()
        {
            return Ok(await trackService.GetAll());
        }

        [HttpGet("Track/GetById{trackId}")]
        public async Task<ActionResult<Track>> GetById(int trackId)
        {
            var track = trackService.GetById(trackId);

            if (track == null)
                return NotFound();

            return Ok(track);
        }

        [HttpPost("Track/Add")]
        public async Task<IActionResult> Add(Track track)
        {
            trackService.Add(track);
            return CreatedAtAction(nameof(GetById), new { trackId = track.TrackId }, track);
        }

        [HttpPut("Track/Update")]
        public async Task<IActionResult> Update(int trackId, Track track)
        {
            if (trackId != track.TrackId)
                return BadRequest();

            var existingTrack = trackService.GetById(trackId);

            if (existingTrack == null)
                return NotFound();

            trackService.Update(track);
            return NoContent();
        }

        [HttpDelete("Track/Delete{trackId}")]
        public async Task<IActionResult> Delete(int trackId)
        {
            var track = trackService.GetById(trackId);

            if (track == null)
                return NotFound();

            trackService.Delete(trackId);

            return NoContent();
        }

        [HttpGet("Track/GetTrack/{trackName}")]

        public ActionResult<List<Track>> GetTrackByName(string trackName)
        {
            return Ok(trackService.GetTrackByName(trackName));
        }

        [HttpGet("Track/GetPlaylist/{playlistId}")]

        public async Task<ActionResult> GetByPlaylistId(int playlistId)
        {
            //var playlist = customerService.LoadListFromDB().Where(e => e.CustomerId== customerId).ToList();
            //if (playlist == null)
            //{
            //    return NotFound();
            //}
            return Ok(await trackService.GetByPlaylistId(playlistId));
        }

        [HttpGet("Track/GetByFilter")]
        public async Task<ActionResult> GetByFilter(int? albumId, int? artistId, int? genreId)
        {
            return Ok(await trackService.GetByFilter(albumId, artistId, genreId));
        }

    }
}
