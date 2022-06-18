using medical_project.Models;
using Microsoft.AspNetCore.Identity;

namespace medical_project
{
    public class AppUser: IdentityUser<int>
    {
        public string FullName { get; set; }
        public string BloodGroup { get; set; } // O+ve, O-ve, AB+ve
        public int Weight { get; set; } //in kg
        public int Height { get; set; } // in cm
        public DateTime DOB { get; set; }


        //Relationship
        public ICollection<AppUserRole> UserRoles { get; set; }
        public ICollection<UserDonatingBlood> UserDonatingBlood { get; set; }
    }
}
