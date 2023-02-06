using Microsoft.EntityFrameworkCore;
using MusicSystem.Models;
using System.Data;
using System.Data.SqlClient;



namespace MusicSystem.Services
{
    public class AlbumService
    {

        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;

        public AlbumService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
        //public class TrackDTO
        //{
        //    public string? Name { get; set; }
        //    public int AlbumId { get; set; }
        //    public int TrackId { get; set; }
        //}



        //public List<TrackDTO> GetAlbumById(int albumId)
        //{
        //    using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        //await conn.OpenAsync();
        //        conn.Open();

        //        SqlCommand query = new SqlCommand("select TrackId, Name, Track.AlbumId from Track, Album where Track.AlbumId = Album.AlbumId and Album.AlbumId = @albumId", _connection);
        //        query.Parameters.Add(new SqlParameter("@albumId", System.Data.SqlDbType.Int));
        //        query.Parameters["@albumId"].Value = albumId;

        //        SqlDataReader reader = query.ExecuteReader();

        //        List<TrackDTO> list = new List<TrackDTO>();
        //        while (reader.Read())
        //        {
        //            list.Add(new TrackDTO
        //            {
        //                Name = reader.GetString("name"),
        //                AlbumId = reader.GetInt32("AlbumId"),
        //                TrackId = reader.GetInt32("TrackId"),
        //            });
        //        }

        //        return list;

        //    }

        //}
        public async Task<List<Album>> GetAll()
        {
            try
            {
                await _connection.OpenAsync();

                SqlCommand query = new SqlCommand("Select * from Album;", _connection);
                SqlDataReader reader = await query.ExecuteReaderAsync();
                List<Album> list = new List<Album>();

                while (reader.Read())
                {
                    list.Add(new Album
                    {
                        Title = reader.GetString("Title"),
                        AlbumId = reader.GetInt32("AlbumId"),
                        ArtistId = reader.GetInt32("ArtistId"),
                    });
                }
                return list;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<List<Album>> GetById(int albumId)
        {
            try
            {
                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("Select * from Album where AlbumId = @albumId", _connection);
                query.Parameters.Add(new SqlParameter("@albumId", System.Data.SqlDbType.Int));
                query.Parameters["@albumId"].Value = albumId;
                SqlDataReader reader = await query.ExecuteReaderAsync();
                List<Album> list = new List<Album>();

                List<Album> fieldValues = new List<Album>();

                while (reader.Read())
                {
                    //reader.GetList(fieldValues);
                    //for (int i = 0; i < fieldValues.Count; i++)
                    //{
                    //    if (Convert.IsDBNull(fieldValues[i]))
                    //        fieldValues[i] = null;
                    //}
                    //list.Add(fieldValues);
                    list.Add(new Album
                    {
                        Title = reader.GetString("Title"),
                        AlbumId = reader.GetInt32("AlbumId"),
                        ArtistId = reader.GetInt32("ArtistId"),
                    });

                    //list.Add(new Album
                    //{
                    //    Title = fieldValues[0].ToString(),
                    //    AlbumId = Convert.ToInt32(fieldValues[1]),
                    //    ArtistId = Convert.ToInt32(fieldValues[2]),
                    //});
                }
                return list;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async void Delete(int albumId)
        {
            try
            {
                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("Delete from Album where AlbumId = @albumId", _connection);
                query.Parameters.AddWithValue("@albumId", albumId);
                await query.ExecuteNonQueryAsync();
            }
            finally
            {
                await _connection.CloseAsync();
            }
            //await _connection.CloseAsync();
        }

        public async void Update(Album album)
        {
            try
            {
                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("UPDATE Album set Title = @title, ArtistId = @artistId where AlbumId = @albumId", _connection);
                query.Parameters.AddWithValue("@title", album.Title);
                query.Parameters.AddWithValue("@artistId", album.ArtistId);
                query.Parameters.AddWithValue("@albumId", album.AlbumId);
                await query.ExecuteNonQueryAsync();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<int> Add(Album album)
        {
            try
            {
                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("INSERT INTO Album VALUES((select AlbumId + 1 from album order by AlbumId offset (select count(*) - 1 from album) Rows fetch next 1 rows only) ,@title, @artistId)", _connection);
                query.Parameters.AddWithValue("@title", album.Title);
                query.Parameters.AddWithValue("@artistId", album.ArtistId);

                return await query.ExecuteNonQueryAsync();
            }
            finally { await _connection.CloseAsync(); }
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

//SqlDataAdapter da = new SqlDataAdapter(cmd);
//DataTable dt = new DataTable();
//da.Fill(dt);
//public static string sqlDataSource = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=world;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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
