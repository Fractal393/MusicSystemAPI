using System.Data;
using System.Data.SqlClient;
using MusicSystem.Models;



namespace MusicSystem.Services
{
    public class PreviousPurchasesService
    {

        private readonly IConfiguration _configuration;
        public PreviousPurchasesService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class InvoiceDTO
        {
            public int CustomerId { get; set; }

            public int InvoiceId { get; set; }

            public int TrackId { get; set; }

            public string Name { get; set; }
        }
        public async Task<List<InvoiceDTO>> GetPreviousPurchases(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //await conn.OpenAsync();
                conn.Open();

                SqlCommand query = new SqlCommand("select CustomerID, Invoice.InvoiceId, Track.TrackId, Name from Track, InvoiceLine, Invoice where Invoice.InvoiceId = InvoiceLine.InvoiceId and InvoiceLine.TrackId = Track.TrackId and CustomerId = @customerId", conn);
                query.Parameters.Add(new SqlParameter("@customerId", System.Data.SqlDbType.Int));
                query.Parameters["@customerId"].Value= customerId;

                //SqlDataReader reader = await query.ExecuteReaderAsync();
                SqlDataReader reader = query.ExecuteReader();

                List<InvoiceDTO> list = new List<InvoiceDTO>();

                while(reader.Read())
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
                //await conn.CloseAsync();


            }

        }
    }
}
