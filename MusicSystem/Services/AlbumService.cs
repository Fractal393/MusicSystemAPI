using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
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

            //public static string sqlDataSource = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=world;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            public List<GetTracksByAlbumId> LoadListFromDB()
            {
                List<GetTracksByAlbumId> lstmain = new List<GetTracksByAlbumId>();

                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                SqlCommand cmd = new SqlCommand("select TrackId, Name, Track.AlbumId from Track, Album where Track.AlbumId = Album.AlbumId", con);
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

                    GetTracksByAlbumId obj = new GetTracksByAlbumId();

                    obj.Name = dt.Rows[i]["Name"].ToString();

                    obj.TrackId = int.Parse(dt.Rows[i]["TrackId"].ToString());

                    obj.AlbumId = int.Parse(dt.Rows[i]["AlbumId"].ToString()) ; 

                    lstmain.Add(obj);
                }

                return lstmain;

            }
}
}
