namespace MusicSystem.Models
{
    public class GetPreviousPurchases
    {
        public int CustomerId { get; set; }

        public int InvoiceId { get; set; }

        public int TrackId { get; set; }

        public string Name { get; set; }
    }
}
