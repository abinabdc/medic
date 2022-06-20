namespace medical_project.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Dose { get; set; } // two tablet after food...
        public string ApplicableFor { get; set; } // minor headache...
        public string Description { get; set; }

        //Relationships
        public int PharmacyId { get; set; }
        public PharmacyDto? SoldBy { get; set; }
    }
   
}
