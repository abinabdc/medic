namespace medical_project.Dtos
{
    public class PharmacyDto
    {
        public int Id { get; set; }
        public string PharmacyName { get; set; }
        public string Location { get; set; }
        public string PANNo { get; set; }
        public string OwnerQualification { get; set; } //eg. Owners qualification

        public int AppUserId { get; set; }
    }
}
