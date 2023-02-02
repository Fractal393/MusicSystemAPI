using Microsoft.AspNetCore.Mvc;
using MusicSystem.Services;
using MusicSystem.Models;
using System.Diagnostics;
using static MusicSystem.Services.AlbumService;
using static MusicSystem.Services.FilterService;
using static MusicSystem.Services.PlaylistService;
using static MusicSystem.Services.PreviousPurchasesService;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicSystem.Controllers
{
    public class MusicSystemController : ControllerBase
    {
        // GET: api/<ValuesController>
        private readonly AlbumService albumService;
        private readonly SongByNameService songByNameService;
        private readonly PlaylistService playlistService;
        private readonly PreviousPurchasesService previousPurchasesService;
        private readonly FilterService filterService;
        private readonly AddAlbumService addAlbumService;
        private readonly DeleteAlbumService deleteAlbumService;
        public MusicSystemController(AlbumService albumService, SongByNameService songByNameService, PlaylistService playlistService, PreviousPurchasesService previousPurchasesService, FilterService filterService, AddAlbumService addAlbumService, DeleteAlbumService deleteAlbumService)
        {
            this.albumService = albumService;
            this.songByNameService = songByNameService;
            this.playlistService = playlistService;
            this.playlistService = playlistService;
            this.previousPurchasesService = previousPurchasesService;
            this.filterService = filterService;
            this.addAlbumService = addAlbumService;
            this.deleteAlbumService = deleteAlbumService;
        }

        [HttpGet("GetAlbum/{albumId}")]
        public ActionResult<List<TrackDTO>> GetAlbums(int albumId)
        {
            return Ok(albumService.GetAlbumById(albumId));
        }

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

        [HttpGet("GetSong/{songName}")]

        public ActionResult<List<Track>> GetSong(string songName)
        {
            return Ok(songByNameService.GetSongByName(songName));
        }

        [HttpGet("GetPlaylist/{playlistId}")]

        public ActionResult<List<PlaylistTrackDTO>> GetPlaylist(int playlistId)
        {
            //var playlist = playlistService.LoadListFromDB().Where(e => e.PlaylistId== playlistId).ToList();
            //if (playlist == null)
            //{
            //    return NotFound();
            //}
            return Ok(playlistService.GetPlaylistSongs(playlistId));
        }

        [HttpGet("GetPreviousPurchases/{customerId}")]
        public ActionResult<List<InvoiceDTO>> GetPrevious(int customerId)
        {
            return Ok(previousPurchasesService.GetPreviousPurchases(customerId));
        }

        [HttpGet("GetByFilter")]
        public ActionResult<List<FilterDTO>> GetFilter(int? albumId, int? artistId, int? genreId)
        {
            return Ok(filterService.GetFilteredSongs(albumId, artistId, genreId));
        }

        [HttpPost("AddAlbum")]
        public ActionResult<List<Album>> AddAlbum(string title, int artistId)
        {
            if (addAlbumService.AddNewAlbum(title, artistId) is not null) { return CreatedAtAction(nameof(GetAlbums), "New Record Inserted"); }
            return NotFound();
        }

        [HttpDelete("DeleteAlbum{albumId}")]
        public ActionResult DeleteAlbum(int albumId) {
            return Ok(deleteAlbumService.DeleteAlbum(albumId)
            );
        }
    }
}