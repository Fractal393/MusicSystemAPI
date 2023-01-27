namespace MusicSystem.Models
{
    public class GetTracksByAlbumId
    {
        public int TrackId { get; set; }
        public string Name { get; set; }

        public int AlbumId { get; set; }
    }
}
