namespace medical_project.Dtos
{
    public class BloodRequestDto
    {
        public int Id { get; set; }
        public string BloodGroup { get; set; }
        public string Status { get; set; } //in-need, expired
        public string RequiredML { get; set; } //470ml is what a normal user can donates
        public string ReceivedML { get; set; }
        public string ExtraComments { get; set; }
        public string Location { get; set; }
        public int AppUserId { get; set; }
    }
}
