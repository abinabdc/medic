namespace medical_project.Dtos
{
    public class OrderProductDto
    {
        public int OrderId { get; set; }
        public ResponseOrderDto Order { get; set; }
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
