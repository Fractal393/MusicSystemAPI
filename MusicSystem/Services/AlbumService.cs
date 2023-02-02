using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Models;



namespace MusicSystem.Services
{
    public class AlbumService
    {

        private readonly IConfiguration _configuration;
        public AlbumService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class TrackDTO
        {
            public string? Name { get; set; }
            public int AlbumId { get; set; }
            public int TrackId { get; set; }
        }
        
        //public static string sqlDataSource = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=world;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<TrackDTO> GetAlbumById(int albumId)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //await conn.OpenAsync();
                conn.Open();

                SqlCommand query = new SqlCommand("select TrackId, Name, Track.AlbumId from Track, Album where Track.AlbumId = Album.AlbumId and Album.AlbumId = @albumId", conn);
                query.Parameters.Add(new SqlParameter("@albumId", System.Data.SqlDbType.Int));
                query.Parameters["@albumId"].Value = albumId;

                SqlDataReader reader = query.ExecuteReader();

                List<TrackDTO> list = new List<TrackDTO>();
                while (reader.Read())
                {
                    list.Add(new TrackDTO
                    {
                        Name = reader.GetString("name"),
                        AlbumId = reader.GetInt32("AlbumId"),
                        TrackId = reader.GetInt32("TrackId"),
                    });
                }

                return list;                
                
            }
            //List<Tracks> lstmain = new List<Tracks>();

            ////SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            //con.Open();
            //SqlCommand cmd = new SqlCommand($"select TrackId, Name, Track.AlbumId from Track, Album where Track.AlbumId = Album.AlbumId and Album.AlbumId = '{albumId}'", con);

            //var reader = cmd.ExecuteReader();

            //while (reader.Read())
            //{
            //    lstmain.Add(new Tracks
            //    {
            //        Name = reader["Name"].ToString(),

            //        TrackId = int.Parse(reader["TrackId"].ToString()),

            //        AlbumId = int.Parse(reader["AlbumId"].ToString()),
            //    });

            //}
            //return lstmain;


        }
    }
}

//SqlDataAdapter da = new SqlDataAdapter(cmd);
//DataTable dt = new DataTable();
//da.Fill(dt);

//Read Data and Display on to the Console.
/*
    SqlDataReader sdr = cmd.ExecuteReader();
    while (sdr.Read())
    {
        for (int i = 0; i < sdr.FieldCount; i++)
        {
            Console.WriteLine(sdr.GetValue(i));
        }
    }
*/


//for (int i = 0; i < dt.Rows.Count; i++)
//{

//    Tracks obj = new Tracks();

//    obj.Name = dt.Rows[i]["Name"].ToString();

//    obj.TrackId = int.Parse(dt.Rows[i]["TrackId"].ToString());

//    obj.AlbumId = int.Parse(dt.Rows[i]["AlbumId"].ToString()) ; 

//    lstmain.Add(obj);
//}

//return lstmain;
