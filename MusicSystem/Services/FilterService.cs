using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using MusicSystem.Models;



namespace MusicSystem.Services
{
    public class FilterService
    {

        private readonly IConfiguration _configuration;
        public FilterService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<GetByFilters> LoadListFromDB()
        {
            List<GetByFilters> lstmain = new List<GetByFilters>();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("select ArtistId, Album.AlbumId, GenreId, Track.Name from Track, Album where Album.AlbumId = Track.AlbumId", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                GetByFilters obj = new GetByFilters();

                obj.Name = dt.Rows[i]["Name"].ToString();

                obj.ArtistId = int.Parse(dt.Rows[i]["ArtistId"].ToString());

                obj.AlbumId = int.Parse(dt.Rows[i]["AlbumId"].ToString());

                obj.GenreId = int.Parse(dt.Rows[i]["GenreId"].ToString());


                lstmain.Add(obj);
            }

            return lstmain;

        }
    }
}
