using Microsoft.AspNetCore.Identity;

namespace medical_project
{
    public class AppUserRole: IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }

    }
}
