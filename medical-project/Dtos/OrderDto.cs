namespace medical_project.Dtos
{
    public class OrderDto
    {
         public int AppUserId { get; set; }
       /* public AppUserDto? AppUser { get; set; }*/
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public string? Status { get; set; } //processing, on the way, delivered
        public string PaymentType { get; set; } // Cash on delivery
        public string? PaymentStatus { get; set; } // Due, Paid
        public DateTime OrderedDate { get; set; }


    }
}
