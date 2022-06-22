using medical_project.Dtos;

namespace medical_project.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Dose { get; set; } // two tablet after food...
        public string ApplicableFor { get; set; } // minor headache...
        public string Description { get; set; }

        //Relationships
        public int PharmacyId { get; set; }
        public Pharmacy SoldBy { get; set; }
        public ICollection<OrderProducts> Orders { get; set; }
    }
}
