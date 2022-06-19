namespace medical_project.Models
{
    public class Pharmacy
    {
        public int Id { get; set; }
        public string PharmacyName { get; set; }
        public string Location { get; set; }
        public string PANNo { get; set; }
        public string OwnerQualification { get; set; } //eg. Owners qualification

        public int AppUserId { get; set; }
        public AppUser Owner { get; set; }

    }
}
