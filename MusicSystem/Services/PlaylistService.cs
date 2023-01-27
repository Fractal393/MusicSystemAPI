using Microsoft.Data.SqlClient;
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

        //public static string sqlDataSource = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=world;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<GetTracksByPlaylist> LoadListFromDB()
        {
            List<GetTracksByPlaylist> lstmain = new List<GetTracksByPlaylist>();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("select PlaylistId, PlaylistTrack.TrackId, Name from Track,PlaylistTrack where PlaylistTrack.TrackId = Track.TrackId", con);
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

                GetTracksByPlaylist obj = new GetTracksByPlaylist();

                obj.Name = dt.Rows[i]["Name"].ToString();

                obj.TrackId = int.Parse(dt.Rows[i]["TrackId"].ToString());

                obj.PlaylistId = int.Parse(dt.Rows[i]["PlaylistId"].ToString());

                lstmain.Add(obj);
            }

            return lstmain;

        }
    }
}
