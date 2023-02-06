using MusicSystem.Models;
using System.Data;
using System.Data.SqlClient;



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
            try
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

                return list;
            }
            finally
            {
                await _connection.CloseAsync();

            }
        }

        public async Task<List<PlaylistTrack>> GetById(int playlistId)
        {
            try
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
                return list;
            }
            finally
            {
                await _connection.CloseAsync();

            }
        }

        public async void Delete(int playlistId)
        {
            try
            {
                await _connection.OpenAsync();

                SqlCommand query = new SqlCommand("Delete from PlaylistTrack where PlaylistId = @playlistId", _connection);
                query.Parameters.AddWithValue("@playlistId", playlistId);
                await query.ExecuteNonQueryAsync();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async void Update(PlaylistTrack PlaylistTrack)
        {
            try
            {

                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("UPDATE PlaylistTrack set TrackId = @trackId where PlaylistId = @playlistId", _connection);
                query.Parameters.AddWithValue("@playlistId", PlaylistTrack.PlaylistId);
                query.Parameters.AddWithValue("@trackId", PlaylistTrack.TrackId);
                await query.ExecuteNonQueryAsync();
            }
            finally
            {
                await _connection.CloseAsync();

            }
        }

        public async Task<int> Add(PlaylistTrack PlaylistTrack)
        {
            try
            {
                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("INSERT INTO PlaylistTrack VALUES(@playlistId ,@trackId)", _connection);
                query.Parameters.AddWithValue("@trackId", PlaylistTrack.TrackId);
                query.Parameters.AddWithValue("@playlistId", PlaylistTrack.PlaylistId);

                return await query.ExecuteNonQueryAsync();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

    }
}
