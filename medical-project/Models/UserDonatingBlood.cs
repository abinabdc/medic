using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_project.Models
{
    public class UserDonatingBlood
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        
       

        public int BloodRequestId { get; set; }
        public BloodRequest BloodRequest { get; set; }


        public int MLDonating { get; set; }
    }
}
