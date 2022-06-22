namespace medical_project.Models
{
    public class OrderProducts
    {
        /*public int OrderProductsId { get; set; }*/

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Status { get; set; } //processing, on the way, delivered
        public string PaymentType { get; set; } // Cash on delivery
        public string PaymentStatus { get; set; } // Due, Paid
        public int Quantity { get; set; }

    }
}
