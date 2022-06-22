namespace medical_project.Models
{
    public class Order
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public string Status { get; set; } //processing, on the way, delivered
        public int PaymentType { get; set; } // Cash on delivery
        public string PaymentStatus { get; set; } // Due, Paid
    }
}