using System.Data;
using System.Data.SqlClient;
using MusicSystem.Models;



namespace MusicSystem.Services
{
    public class PlaylistService
    {

        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;

        public PlaylistService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        }

        public class PlaylistTrackDTO
        {
            public int PlaylistId { get; set; }
            public int TrackId { get; set; }
            public string? Name { get; set; }
        }

        public async Task<List<PlaylistTrack>> GetAll()
        {
            await _connection.OpenAsync();

            SqlCommand query = new SqlCommand("Select * from PlaylistTrack", _connection);
            SqlDataReader reader = await query.ExecuteReaderAsync();
            List<PlaylistTrack> list = new List<PlaylistTrack>();

            while (reader.Read())
            {
                list.Add(new PlaylistTrack
                {
                    TrackId = reader.GetInt32("TrackId"),
                    PlaylistId = reader.GetInt32("PlaylistId"),
                });
            }

            await _connection.CloseAsync();
            return list;
        }

        public async Task<List<PlaylistTrack>> GetById(int playlistId)
        {
            await _connection.OpenAsync();
            SqlCommand query = new SqlCommand("Select * from PlaylistTrack where PlaylistId = @playlistId", _connection);
            query.Parameters.AddWithValue("@playlistId", playlistId);
            SqlDataReader reader = await query.ExecuteReaderAsync();
            List<PlaylistTrack> list = new List<PlaylistTrack>();

            while (reader.Read())
            {
                list.Add(new PlaylistTrack
                {
                    TrackId = reader.GetInt32("TrackId"),
                    PlaylistId = reader.GetInt32("PlaylistId"),
                });
            }
            await _connection.CloseAsync();
            return list;
        }

        public async void Delete(int playlistId)
        {
            await _connection.OpenAsync();
            SqlCommand query = new SqlCommand("Delete from PlaylistTrack where PlaylistId = @playlistId", _connection);
            query.Parameters.AddWithValue("@playlistId", playlistId);
            await query.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public async void Update(PlaylistTrack PlaylistTrack)
        {
            await _connection.OpenAsync();
            SqlCommand query = new SqlCommand("UPDATE PlaylistTrack set TrackId = @trackId where PlaylistId = @playlistId", _connection);
            query.Parameters.AddWithValue("@playlistId", PlaylistTrack.PlaylistId);
            query.Parameters.AddWithValue("@trackId", PlaylistTrack.TrackId);
            await query.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public async void Add(PlaylistTrack PlaylistTrack)
        {
            await _connection.OpenAsync();
            SqlCommand query = new SqlCommand("INSERT INTO PlaylistTrack VALUES(@playlistId ,@trackId)", _connection);
            query.Parameters.AddWithValue("@trackId", PlaylistTrack.TrackId);
            query.Parameters.AddWithValue("@playlistId", PlaylistTrack.PlaylistId);
            await query.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }   
       
    }
}
