using System.Data;
using System.Data.SqlClient;
using MusicSystem.Models;



namespace MusicSystem.Services
{
    public class PlaylistService
    {

        private readonly IConfiguration _configuration;
        public PlaylistService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class PlaylistTrackDTO
        {
            public int PlaylistId { get; set; }
            public int TrackId { get; set; }
            public string Name { get; set; }
        }

        //public static string sqlDataSource = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=world;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public async Task<List<PlaylistTrackDTO>> GetPlaylistSongs(int playlistId)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //await conn.OpenAsync();
                conn.Open();

                SqlCommand query = new SqlCommand("select PlaylistId, PlaylistTrack.TrackId, Name from Track,PlaylistTrack where PlaylistTrack.TrackId = Track.TrackId and PlaylistId = @playlistId", conn);
                query.Parameters.Add(new SqlParameter("@playlistId", System.Data.SqlDbType.Int));
                query.Parameters["@playlistId"].Value = playlistId;

                //SqlDataReader reader = await query.ExecuteReaderAsync();
                SqlDataReader reader = query.ExecuteReader();

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
                //await conn.CloseAsync();

            }

        }
    }
}
