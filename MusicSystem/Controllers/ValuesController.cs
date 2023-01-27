using Microsoft.AspNetCore.Mvc;
using MusicSystem.Services;
using MusicSystem.Models;
using System.Diagnostics;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicSystem.Controllers
{
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        private readonly AlbumService albumService;
        private readonly SongByNameService songByNameService;
        private readonly PlaylistService playlistService; 
        private readonly PreviousPurchasesService previousPurchasesService;
        private readonly FilterService filterService;
        public ValuesController(AlbumService albumService, SongByNameService songByNameService, PlaylistService playlistService, PreviousPurchasesService previousPurchasesService, FilterService filterService)
        {
            this.albumService = albumService;
            this.songByNameService = songByNameService;
            this.playlistService = playlistService;
            this.playlistService = playlistService;
            this.previousPurchasesService = previousPurchasesService;
            this.filterService = filterService;
        }

        [HttpGet("GetAlbum")]
        public ActionResult<List<GetTracksByAlbumId>> GetAlbums()
        {
            return Ok(albumService.LoadListFromDB());
        }

        // GET api/<ValuesController>/5
        [HttpGet("GetAlbum/{albumId}")]
        public ActionResult<List<GetTracksByAlbumId>> GetAlbum(int albumId)
        {
            var album = albumService.LoadListFromDB().Where(e => e.AlbumId == albumId).ToList();
            if (album == null)
            {
                return NotFound();
            }
            return Ok(album);  
         
        }

        [HttpGet("GetSong/{songName}")]

        public ActionResult<List<GetSongByName>> GetSong(string songName)
        {
            var song = songByNameService.LoadListFromDB().Where(e => e.Name == songName).ToList();
            if (song == null)
            {
                return NotFound();
            }
            return Ok(song);    
        }

        [HttpGet("GetPlaylist/{playlistId}")]

        public ActionResult<List<GetTracksByPlaylist>> GetPlaylist(int playlistId)
        {
            var playlist = playlistService.LoadListFromDB().Where(e => e.PlaylistId== playlistId).ToList();
            if (playlist == null)
            {
                return NotFound();
            }
                return Ok(playlist);
        }

        [HttpGet("GetPreviousPurchases/{customerId}")]

        public ActionResult<List<GetPreviousPurchases>> GetPrevious(int customerId)
        {
            var purchase = previousPurchasesService.LoadListFromDB().Where(e => e.CustomerId == customerId).ToList();
            if (purchase == null)
            {
                return NotFound();  
            }
            return Ok(purchase);
        }

        [HttpGet("GetByFilter")]

        public ActionResult<List<GetByFilters>> GetFilter(int genreId, int artistId, int albumId)
        {
            var genre = filterService.LoadListFromDB().Where(e => e.GenreId == genreId).ToList();
            var artist = filterService.LoadListFromDB().Where(e => e.ArtistId == artistId).ToList();
            var album = filterService.LoadListFromDB().Where(e => e.AlbumId == albumId).ToList();
            
            if (genre != null)
            {
                return Ok(genre);
            }
            else if(artist != null) {
                return Ok(artist);
            }
            else return Ok(album);
            
        }
    }
}
