﻿using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Models;



namespace MusicSystem.Services
{
    public class DeleteAlbumService
    {

        private readonly IConfiguration _configuration;
        public DeleteAlbumService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //public static string sqlDataSource = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=world;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<Album> DeleteAlbum(int albumId)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //await conn.OpenAsync();
                conn.Open();

                SqlCommand query = new SqlCommand("Delte from Album where AlbumId = @albumId", conn);
                query.Parameters.Add(new SqlParameter("@albumId", System.Data.SqlDbType.Int));
                query.Parameters["@albumId"].Value = albumId;

                SqlDataReader reader = query.ExecuteReader();

                List<Album> list = new List<Album>();
                while (reader.Read())
                {
                    list.Add(new Album
                    {
                        //ArtistId = reader.GetInt32("ArtistId"),
                        AlbumId = reader.GetInt32("AlbumId"),
                        //Title = reader.GetString("Title"),
                    });
                }
                return list;

            }

        }
    }
}