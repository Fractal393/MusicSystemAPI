using MusicSystem.Models;
using System.Data;
using System.Data.SqlClient;
using static MusicSystem.Services.PlaylistService;

namespace MusicSystem.Services
{
    public static class ExtensionInt
    {
        public static int GetIntOrDefault(this SqlDataReader reader, string fieldName)
        {
            int colIndex = reader.GetOrdinal(fieldName);
            if (!reader.IsDBNull(colIndex))
            {
                return reader.GetInt32(colIndex);
            }
            return 0;
        }
    }
    public class TrackService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;

        public TrackService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
        public async Task<List<Track>> GetAll()
        {
            try
            {

                await _connection.OpenAsync();

                SqlCommand query = new SqlCommand("Select * from Track", _connection);
                SqlDataReader reader = await query.ExecuteReaderAsync();
                List<Track> list = new List<Track>();

                while (reader.Read())
                {
                    list.Add(new Track
                    {
                        TrackId = reader.GetInt32("TrackId"),
                        Name = reader.GetString("name"),
                        AlbumId = reader.GetInt32("AlbumId"),
                        MediaTypeId = reader.GetInt32("MediaTypeId"),
                        GenreId = reader.GetInt32("GenreId"),
                        Composer = reader.GetString("Composer"),
                        Milliseconds = reader.GetInt32("Milliseconds"),
                        Bytes = reader.GetInt32("Bytes"),
                        UnitPrice = reader.GetDecimal("UnitPrice")
                    });
                }

                return list;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<Track>> GetById(int trackId)
        {
            try
            {

                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("Select * from Track where TrackId = @trackId", _connection);
                query.Parameters.AddWithValue("@trackId", trackId);
                SqlDataReader reader = await query.ExecuteReaderAsync();
                List<Track> list = new List<Track>();

                while (reader.Read())
                {
                    list.Add(new Track
                    {
                        TrackId = reader.GetInt32("TrackId"),
                        Name = reader.GetString("name"),
                        AlbumId = reader.GetIntOrDefault("AlbumId"),
                        MediaTypeId = reader.GetInt32("MediaTypeId"),
                        GenreId = reader.GetIntOrDefault("GenreId"),
                        Composer = reader.GetValueOrDefault("Composer"),
                        Milliseconds = reader.GetInt32("Milliseconds"),
                        Bytes = reader.GetIntOrDefault("Bytes"),
                        UnitPrice = reader.GetDecimal("UnitPrice")
                    });
                }
                return list;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async void Delete(int trackId)
        {
            try
            {

                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("Delete from Track where TrackId = @trackId", _connection);
                query.Parameters.AddWithValue("@trackId", trackId);
                await query.ExecuteNonQueryAsync();
            }
            finally
            {
                await _connection.CloseAsync();

            }
        }

        public async void Update(Track Track)
        {
            try
            {

                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("UPDATE Track set Composer = @composer where TrackId = @trackId", _connection);
                query.Parameters.AddWithValue("@trackId", Track.TrackId);
                query.Parameters.AddWithValue("@composer", Track.Composer);
                await query.ExecuteNonQueryAsync();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<int> Add(Track Track)
        {
            try
            {

                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("INSERT INTO Track VALUES((select AlbumId + 1 from album order by AlbumId offset (select count(*) - 1 from album) Rows fetch next 1 rows only), @Name, @AlbumId , @MediaTypeId , @GenreId , @Composer, @Milliseconds, @Bytes , @UnitPrice)", _connection);
                query.Parameters.AddWithValue("@Name", Track.Name);
                query.Parameters.AddWithValue("@AlbumId", Track.AlbumId);
                query.Parameters.AddWithValue("@MediaTypeId", Track.MediaTypeId);
                query.Parameters.AddWithValue("@GenreId", Track.GenreId);
                query.Parameters.AddWithValue("@Milliseconds", Track.Milliseconds);
                query.Parameters.AddWithValue("@Bytes", Track.Bytes);
                query.Parameters.AddWithValue("@Composer", Track.Composer);
                query.Parameters.AddWithValue("@UnitPrice", Track.UnitPrice);
                return await query.ExecuteNonQueryAsync();
            }
            finally
            {

                await _connection.CloseAsync();
            }
        }

        public async Task<List<Track>> GetTrackByName(string track)
        {
            try
            {

                await _connection.OpenAsync();

                SqlCommand query = new SqlCommand("select * from Track where Name = @track", _connection);
                query.Parameters.AddWithValue("@track", track);

                SqlDataReader reader = query.ExecuteReader();

                List<Track> list = new List<Track>();
                while (reader.Read())
                {
                    list.Add(new Track
                    {
                        Name = reader.GetString("name"),
                        AlbumId = reader.GetInt32("AlbumId"),
                        MediaTypeId = reader.GetInt32("MediaTypeId"),
                        GenreId = reader.GetInt32("GenreId"),
                        Composer = reader.GetString("Composer"),
                        Milliseconds = reader.GetInt32("Milliseconds"),
                        Bytes = reader.GetInt32("Bytes"),
                        UnitPrice = reader.GetDecimal("UnitPrice")
                    });
                }

                return list;
            }
            finally
            {
                await _connection.CloseAsync();

            }

        }

        public class FilterDTO
        {
            public int ArtistId { get; set; }
            public int AlbumId { get; set; }
            public int GenreId { get; set; }
            //public int PlaylistId { get; set; }
            public string? Name { get; set; }

        }

        public async Task<List<FilterDTO>> GetByFilter(int? albumId, int? artistId, int? genreId)
        {
            try
            {


                await _connection.OpenAsync();

                //SqlCommand query = new SqlCommand("select ArtistId, Album.AlbumId, GenreId, Track.Name from Track, Album where Album.AlbumId = Track.AlbumId and (@genreId is NULL or GenreId = @genreId) and (@albumid is NULL or Album.AlbumId = @albumId) and (@artistId is NULL or ArtistId = @artistId)", conn);
                SqlCommand query = new SqlCommand("DECLARE @sql NVARCHAR(MAX) = 'SELECT Album.ArtistId, Album.AlbumId, Track.GenreId, Track.Name\r\nFROM Track\r\nJOIN Album ON Track.AlbumId = Album.AlbumId\r\nWHERE 1=1'\r\n\r\nIF (@albumId IS NOT NULL)\r\nBEGIN\r\n    SET @sql = @sql + ' AND Album.AlbumId = @albumId'\r\nEND\r\n\r\nIF (@artistId IS NOT NULL)\r\nBEGIN\r\n    SET @sql = @sql + ' AND Album.ArtistId = @artistId'\r\nEND\r\n\r\nIF (@genreId IS NOT NULL)\r\nBEGIN\r\n    SET @sql = @sql + ' AND Track.GenreId = @genreId'\r\nEND\r\n\r\nDECLARE @params NVARCHAR(MAX) = N'@albumId INT, @artistId INT, @genreId INT'\r\nDECLARE @albumIdParam INT = COALESCE(@albumId, NULL)\r\nDECLARE @artistIdParam INT = COALESCE(@artistId, NULL)\r\nDECLARE @genreIdParam INT = COALESCE(@genreId, NULL)\r\n\r\nEXEC sp_executesql @sql, @params, @albumIdParam, @artistIdParam, @genreIdParam;\r\n", _connection);


                SqlParameter albumIdParam = query.Parameters.AddWithValue("@albumId", albumId);
                if (albumId == null)
                {
                    albumIdParam.Value = DBNull.Value;
                }
                SqlParameter artistIdParam = query.Parameters.AddWithValue("@artistId", artistId);
                if (artistId == null)
                {
                    artistIdParam.Value = DBNull.Value;
                }
                SqlParameter genreIdParam = query.Parameters.AddWithValue("@genreId", genreId);
                if (genreId == null)
                {
                    genreIdParam.Value = DBNull.Value;
                }

                //SqlDataReader reader = await query.ExecuteReaderAsync();
                SqlDataReader reader = await query.ExecuteReaderAsync();

                List<FilterDTO> list = new List<FilterDTO>();

                while (reader.Read())
                {

                    list.Add(new FilterDTO
                    {
                        Name = reader.GetString("name"),
                        AlbumId = reader.GetInt32("AlbumId"),
                        ArtistId = reader.GetInt32("ArtistId"),
                        GenreId = reader.GetInt32("GenreId"),
                    });
                }

                return list;
            }
            finally
            {
                await _connection.CloseAsync();

            }
        }

        public async Task<List<PlaylistTrackDTO>> GetByPlaylistId(int playlistId)
        {
            try
            {

                await _connection.OpenAsync();

                SqlCommand query = new SqlCommand("select PlaylistId, PlaylistTrack.TrackId, Name from Track,PlaylistTrack where PlaylistTrack.TrackId = Track.TrackId and PlaylistId = @playlistId", _connection);
                query.Parameters.Add(new SqlParameter("@playlistId", System.Data.SqlDbType.Int));
                query.Parameters["@playlistId"].Value = playlistId;

                //SqlDataReader reader = await query.ExecuteReaderAsync();
                SqlDataReader reader = await query.ExecuteReaderAsync();

                List<PlaylistTrackDTO> list = new List<PlaylistTrackDTO>();

                while (reader.Read())
                {
                    list.Add(new PlaylistTrackDTO
                    {
                        Name = reader.GetString("name"),
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
    }


}




