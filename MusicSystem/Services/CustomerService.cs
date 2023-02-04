using MusicSystem.Models;
using System.Data;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Diagnostics.Metrics;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MusicSystem.Services
{
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
            await _connection.OpenAsync();

            SqlCommand query = new SqlCommand("Select * from Customer", _connection);
            SqlDataReader reader = await query.ExecuteReaderAsync();
            List<Customer> list = new List<Customer>();

            while (reader.Read())
            {
                list.Add(new Customer
                {
                    CustomerId    = reader.GetInt32("CustomerId"),
                    FirstName    = reader.GetString("FirstName"),
                    LastName     = reader.GetString("LastName"),
                    Company      = reader.GetString("Company"),
                    Address      = reader.GetString("Address"),
                    City         = reader.GetString("City"),
                    State        = reader.GetString("State"),
                    Country      = reader.GetString("Country"),
                    PostalCode   = reader.GetString("PostalCode"),
                    Phone        = reader.GetString("Phone"),
                    Fax          = reader.GetString("Fax"),
                    Email        = reader.GetString("Email"),
                    SupportRepId  = reader.GetInt32("SupportRepId")
                });
            }

            await _connection.CloseAsync();
            return list;
        }

        public async Task<List<Customer>> GetById(int customerId)
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
            await _connection.CloseAsync();
            return list;
        }

        public async void Delete(int customerId)
        {
            await _connection.OpenAsync();
            SqlCommand query = new SqlCommand("Delete from Customer where CustomerId = @customerId", _connection);
            query.Parameters.AddWithValue("@customerId", customerId);
            await query.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public async void Update(Customer Customer)
        {
            await _connection.OpenAsync();
            SqlCommand query = new SqlCommand("UPDATE Customer set Email = @email where CustomerId = @customerId", _connection);
            query.Parameters.AddWithValue("@customerId", Customer.CustomerId);
            query.Parameters.AddWithValue("@email", Customer.Email);
            await query.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public async void Add(Customer Customer)
        {
            await _connection.OpenAsync();
            SqlCommand query = new SqlCommand("INSERT INTO Customer VALUES(@CustomerId, @FirstName, @LastName, @Company, @Address, @City, @State, @Country, @PostalCode, @Phone, @Fax, @Email, @SupportRepId)", _connection);
            query.Parameters.AddWithValue("@CustomerId", Customer.CustomerId);
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
            await query.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        public async Task<List<InvoiceDTO>> GetPreviousPurchases(int customerId)
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

            await _connection.CloseAsync();
            return list;


            }

    }
}
