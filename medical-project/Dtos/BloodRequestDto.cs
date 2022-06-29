using medical_project.Models;

namespace medical_project.Dtos
{
    public class BloodRequestDto
    {
        public int Id { get; set; }
        public string BloodGroup { get; set; }
        public bool isExpired { get; set; } //in-need, expired
        public int RequiredML { get; set; } //470ml is what a normal user can donates
        public int ReceivedML { get; set; }
        public string ExtraComments { get; set; }
        public string Location { get; set; }
        public int AppUserId { get; set; }
        public ICollection<UserDonatingBloodDto>? UsersDonatingBlood { get; set; }
    }
}
