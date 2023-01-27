using Microsoft.Data.SqlClient;
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

        public List<GetPreviousPurchases> LoadListFromDB()
        {
            List<GetPreviousPurchases> lstmain = new List<GetPreviousPurchases>();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("select CustomerID, Invoice.InvoiceId, Track.TrackId, Name from Track, InvoiceLine, Invoice where Invoice.InvoiceId = InvoiceLine.InvoiceId and InvoiceLine.TrackId = Track.TrackId", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                GetPreviousPurchases obj = new GetPreviousPurchases();

                obj.Name = dt.Rows[i]["Name"].ToString();

                obj.TrackId = int.Parse(dt.Rows[i]["TrackId"].ToString());

                obj.InvoiceId = int.Parse(dt.Rows[i]["InvoiceId"].ToString());

                obj.CustomerId = int.Parse(dt.Rows[i]["CustomerId"].ToString());



                lstmain.Add(obj);
            }

            return lstmain;

        }
    }
}
