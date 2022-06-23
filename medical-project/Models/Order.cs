using System.ComponentModel.DataAnnotations;

namespace medical_project.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int AppUserId { get; set; }
        
        public int TotalPrice { get; set; }
        public string PayStatus { get; set; }// Due, Paid
        public string PayType { get; set; }// Cash on delivery
        public string OrderStatus { get; set; } //processing, on the way, delivered


        public ICollection<OrderProducts> productsInOrder { get; set; }
    }
}