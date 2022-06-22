using System.ComponentModel.DataAnnotations;

namespace medical_project.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int AppUserId { get; set; }
        
        public int TotalPrice { get; set; }





        public ICollection<OrderProducts> productsInOrder { get; set; }
    }
}