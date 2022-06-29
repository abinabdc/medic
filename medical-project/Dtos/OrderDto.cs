namespace medical_project.Dtos
{
    public class OrderDto
    {
        public int TotalPrice { get; set; }
        public List<OrdersArray> product { get; set; }

    }
    public class OrdersArray
    {
        public int product_id { get; set; }
        public int quantity { get; set; }
    }

}
