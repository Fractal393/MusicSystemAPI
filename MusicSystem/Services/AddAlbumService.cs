using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Models;



namespace MusicSystem.Services
{
    public class AddAlbumService
    {

        private readonly IConfiguration _configuration;
        public AddAlbumService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public static string sqlDataSource = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=world;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public async Task<List<Album>> AddNewAlbum(string title, int artistId)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await conn.OpenAsync();
                //conn.Open();

                SqlCommand query = new SqlCommand("Insert into Album values((select count(*) + 1 from Album),@title, @artistId)", conn);
                //query.Parameters.Add(new SqlParameter("@albumId", System.Data.SqlDbType.Int));
                //query.Parameters["@albumId"].Value = albumId;
                query.Parameters.Add(new SqlParameter("@title", System.Data.SqlDbType.VarChar));
                query.Parameters["@title"].Value = title;
                query.Parameters.Add(new SqlParameter("@artistId", System.Data.SqlDbType.Int));
                query.Parameters["@artistId"].Value = artistId;

                SqlDataReader reader = await query.ExecuteReaderAsync();

                List<Album> list = new List<Album>();
                while (reader.Read())
                {
                    list.Add(new Album
                    {
                        ArtistId = reader.GetInt32("ArtistId"),
                        //AlbumId = reader.GetInt32("AlbumId"),
                        Title = reader.GetString("Title"),
                    });
                }
                return list;

            }

        }
    }
}
            