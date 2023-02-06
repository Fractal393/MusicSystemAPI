using MusicSystem.Models;
using System.Data;
using System.Data.SqlClient;

namespace MusicSystem.Services
{
    public static class Extension
    {
        public static string? GetValueOrDefault(this SqlDataReader reader, string fieldName)
        {
            int colIndex = reader.GetOrdinal(fieldName);
            if (!reader.IsDBNull(colIndex))
            {
                return reader.GetString(colIndex);
            }
            return null;
        }
    }

    public class CustomerService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;

        public CustomerService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
        public class InvoiceDTO
        {
            public int CustomerId { get; set; }

            public int InvoiceId { get; set; }

            public int TrackId { get; set; }

            public string? Name { get; set; }
        }
        public async Task<List<Customer>> GetAll()
        {
            try
            {
                await _connection.OpenAsync();

                SqlCommand query = new SqlCommand("Select * from Customer", _connection);
                SqlDataReader reader = await query.ExecuteReaderAsync();
                List<Customer> list = new List<Customer>();

                while (reader.Read())
                {
                    list.Add(new Customer
                    {
                        CustomerId = reader.GetInt32("CustomerId"),
                        FirstName = reader.GetValueOrDefault("FirstName"),
                        LastName = reader.GetValueOrDefault("LastName"),
                        Company = reader.GetValueOrDefault("Company"),
                        Address = reader.GetValueOrDefault("Address"),
                        City = reader.GetValueOrDefault("City"),
                        State = reader.GetValueOrDefault("State"),
                        Country = reader.GetValueOrDefault("Country"),
                        PostalCode = reader.GetValueOrDefault("PostalCode"),
                        Phone = reader.GetValueOrDefault("Phone"),
                        Fax = reader.GetValueOrDefault("Fax"),
                        //Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                        Email = reader.GetValueOrDefault("Email"),
                        SupportRepId = reader.GetInt32("SupportRepId")
                    });
                }


                return list;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<Customer>> GetById(int customerId)
        {
            try
            {
                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("Select * from Customer where CustomerId = @customerId", _connection);
                query.Parameters.AddWithValue("@customerId", customerId);
                SqlDataReader reader = await query.ExecuteReaderAsync();
                List<Customer> list = new List<Customer>();

                while (reader.Read())
                {
                    list.Add(new Customer
                    {
                        CustomerId = reader.GetInt32("CustomerId"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Company = reader.GetString("Company"),
                        Address = reader.GetString("Address"),
                        City = reader.GetString("City"),
                        State = reader.GetString("State"),
                        Country = reader.GetString("Country"),
                        PostalCode = reader.GetString("PostalCode"),
                        Phone = reader.GetString("Phone"),
                        Fax = reader.GetString("Fax"),
                        Email = reader.GetString("Email"),
                        SupportRepId = reader.GetInt32("SupportRepId")
                    });
                }
                return list;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async void Delete(int customerId)
        {
            try
            {
                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("Delete from Customer where CustomerId = @customerId", _connection);
                query.Parameters.AddWithValue("@customerId", customerId);
                await query.ExecuteNonQueryAsync();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async void Update(Customer Customer)
        {
            try
            {

                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("UPDATE Customer set Email = @email where CustomerId = @customerId", _connection);
                query.Parameters.AddWithValue("@customerId", Customer.CustomerId);
                query.Parameters.AddWithValue("@email", Customer.Email);
                await query.ExecuteNonQueryAsync();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<int> Add(Customer Customer)
        {
            try
            {

                await _connection.OpenAsync();
                SqlCommand query = new SqlCommand("INSERT INTO Customer VALUES((select AlbumId + 1 from album order by AlbumId offset (select count(*) - 1 from album) Rows fetch next 1 rows only), @FirstName, @LastName, @Company, @Address, @City, @State, @Country, @PostalCode, @Phone, @Fax, @Email, @SupportRepId)", _connection);
                query.Parameters.AddWithValue("@FirstName", Customer.FirstName);
                query.Parameters.AddWithValue("@LastName", Customer.LastName);
                query.Parameters.AddWithValue("@Company", Customer.Company);
                query.Parameters.AddWithValue("@Address", Customer.Address);
                query.Parameters.AddWithValue("@City", Customer.City);
                query.Parameters.AddWithValue("@State", Customer.State);
                query.Parameters.AddWithValue("@Country", Customer.Country);
                query.Parameters.AddWithValue("@PostalCode", Customer.PostalCode);
                query.Parameters.AddWithValue("@Phone", Customer.Phone);
                query.Parameters.AddWithValue("@Fax", Customer.Fax);
                query.Parameters.AddWithValue("@Email", Customer.Email);
                query.Parameters.AddWithValue("@SupportRepId", Customer.SupportRepId);
                return await query.ExecuteNonQueryAsync();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<InvoiceDTO>> GetPreviousPurchases(int customerId)
        {
            try
            {

                await _connection.OpenAsync();

                SqlCommand query = new SqlCommand("select CustomerID, Invoice.InvoiceId, Track.TrackId, Name from Track, InvoiceLine, Invoice where Invoice.InvoiceId = InvoiceLine.InvoiceId and InvoiceLine.TrackId = Track.TrackId and CustomerId = @customerId", _connection);
                query.Parameters.AddWithValue("@customerId", customerId);

                //SqlDataReader reader = await query.ExecuteReaderAsync();
                SqlDataReader reader = query.ExecuteReader();

                List<InvoiceDTO> list = new List<InvoiceDTO>();

                while (reader.Read())
                {
                    list.Add(new InvoiceDTO
                    {
                        Name = reader.GetString("name"),
                        InvoiceId = reader.GetInt32("InvoiceId"),
                        TrackId = reader.GetInt32("TrackId"),
                        CustomerId = reader.GetInt32("CustomerId"),

                    });
                }

                return list;
            }
            finally
            {
                await _connection.CloseAsync();
            }


        }

    }
}
