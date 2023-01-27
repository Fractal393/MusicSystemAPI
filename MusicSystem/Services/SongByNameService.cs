using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using MusicSystem.Models;


namespace MusicSystem.Services
{
    public class SongByNameService
    {

        private readonly IConfiguration _configuration;
        public SongByNameService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public static string sqlDataSource = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=world;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<GetSongByName> LoadListFromDB()
        {
            List<GetSongByName> lstmain = new List<GetSongByName>();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("select * from Track", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            //SqlDataReader sdr = cmd.ExecuteReader();
            //while (sdr.Read())
            //{
            //    for (int i = 0; i < sdr.FieldCount; i++)
            //    {
            //        Console.WriteLine(sdr.GetValue(i));
            //    }
            //}


            for (int i = 0; i < dt.Rows.Count; i++)
            {

                GetSongByName obj = new GetSongByName();

                obj.TrackId = int.Parse(dt.Rows[i]["TrackId"].ToString());

                obj.Name = dt.Rows[i]["Name"].ToString();

                obj.AlbumId = int.Parse(dt.Rows[i]["AlbumId"].ToString());

                obj.MediaTypeId = int.Parse(dt.Rows[i]["MediaTypeId"].ToString());

                obj.GenreId = int.Parse(dt.Rows[i]["GenreId"].ToString());

                obj.Composer = dt.Rows[i]["Composer"].ToString();

                obj.Milliseconds = int.Parse(dt.Rows[i]["Milliseconds"].ToString());

                obj.Bytes = int.Parse(dt.Rows[i]["Bytes"].ToString());

                obj.UnitPrice = decimal.Parse(dt.Rows[i]["UnitPrice"].ToString());

                lstmain.Add(obj);
            }

            return lstmain;

        }
    }
}
