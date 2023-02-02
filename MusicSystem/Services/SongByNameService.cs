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

        public List<Track> GetSongByName(string song)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                SqlCommand query = new SqlCommand("select * from Track where Name = @song", conn);
                query.Parameters.Add(new SqlParameter("@song", System.Data.SqlDbType.VarChar));
                query.Parameters["@song"].Value = song;

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
        }
    }
}

//            return list;

//            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
//            SqlDataAdapter da = new SqlDataAdapter(cmd);
//            DataTable dt = new DataTable();
//            da.Fill(dt);


//            //SqlDataReader sdr = cmd.ExecuteReader();
//            //while (sdr.Read())
//            //{
//            //    for (int i = 0; i < sdr.FieldCount; i++)
//            //    {
//            //        Console.WriteLine(sdr.GetValue(i));
//            //    }
//            //}


//            for (int i = 0; i < dt.Rows.Count; i++)
//            {

//                Track obj = new Track();

//                obj.TrackId = int.Parse(dt.Rows[i]["TrackId"].ToString());

//                obj.Name = dt.Rows[i]["Name"].ToString();

//                obj.AlbumId = int.Parse(dt.Rows[i]["AlbumId"].ToString());

//                obj.MediaTypeId = int.Parse(dt.Rows[i]["MediaTypeId"].ToString());

//                obj.GenreId = int.Parse(dt.Rows[i]["GenreId"].ToString());

//                obj.Composer = dt.Rows[i]["Composer"].ToString();

//                obj.Milliseconds = int.Parse(dt.Rows[i]["Milliseconds"].ToString());

//                obj.Bytes = int.Parse(dt.Rows[i]["Bytes"].ToString());

//                obj.UnitPrice = decimal.Parse(dt.Rows[i]["UnitPrice"].ToString());

//                lstmain.Add(obj);
//            }

//            return lstmain;

//        }
//    }
//}
