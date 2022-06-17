using System.ComponentModel.DataAnnotations.Schema;

namespace medical_project.Models
{
    public class UserDonatingBlood
    {
        public int AppUserId { get; set; }
        public AppUser UserDonating { get; set; }
        public int BloodRequestId { get; set; }
        public BloodRequest BloodRequestPost { get; set; }
    }
}
