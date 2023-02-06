using System.Data;
using System.Data.SqlClient;



namespace MusicSystem.Services
{
    public class FilterService
    {

        private readonly IConfiguration _configuration;
        public FilterService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class FilterDTO
        {
            public int ArtistId { get; set; }
            public int AlbumId { get; set; }
            public int GenreId { get; set; }
            //public int PlaylistId { get; set; }
            public string? Name { get; set; }

        }

        public async Task<List<FilterDTO>> GetFilteredSongs(int? albumId, int? artistId, int? genreId)
        {

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await conn.OpenAsync();
                conn.Open();

                //SqlCommand query = new SqlCommand("select ArtistId, Album.AlbumId, GenreId, Track.Name from Track, Album where Album.AlbumId = Track.AlbumId and (@genreId is NULL or GenreId = @genreId) and (@albumid is NULL or Album.AlbumId = @albumId) and (@artistId is NULL or ArtistId = @artistId)", conn);
                SqlCommand query = new SqlCommand("DECLARE @sql NVARCHAR(MAX) = 'SELECT Album.ArtistId, Album.AlbumId, Track.GenreId, Track.Name\r\nFROM Track\r\nJOIN Album ON Track.AlbumId = Album.AlbumId\r\nWHERE 1=1'\r\n\r\nIF (@albumId IS NOT NULL)\r\nBEGIN\r\n    SET @sql = @sql + ' AND Album.AlbumId = @albumId'\r\nEND\r\n\r\nIF (@artistId IS NOT NULL)\r\nBEGIN\r\n    SET @sql = @sql + ' AND Album.ArtistId = @artistId'\r\nEND\r\n\r\nIF (@genreId IS NOT NULL)\r\nBEGIN\r\n    SET @sql = @sql + ' AND Track.GenreId = @genreId'\r\nEND\r\n\r\nDECLARE @params NVARCHAR(MAX) = N'@albumId INT, @artistId INT, @genreId INT'\r\nDECLARE @albumIdParam INT = COALESCE(@albumId, NULL)\r\nDECLARE @artistIdParam INT = COALESCE(@artistId, NULL)\r\nDECLARE @genreIdParam INT = COALESCE(@genreId, NULL)\r\n\r\nEXEC sp_executesql @sql, @params, @albumIdParam, @artistIdParam, @genreIdParam;\r\n", conn);
                //query.Parameters.Add(new SqlParameter("@genreId", System.Data.SqlDbType.Int));
                //query.Parameters["@genreId"].Value = genreId;
                //query.Parameters.Add(new SqlParameter("@albumId", System.Data.SqlDbType.Int));
                //query.Parameters["@albumId"].Value = albumId;
                //query.Parameters.Add(new SqlParameter("@artistId", System.Data.SqlDbType.Int));
                //query.Parameters["@artistId"].Value = artistId;

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
                SqlDataReader reader = query.ExecuteReader();

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
                //await conn.CloseAsync();
            }
        }
    }
}
