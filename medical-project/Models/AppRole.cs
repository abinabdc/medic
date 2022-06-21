using Microsoft.AspNetCore.Identity;

namespace medical_project
{
    public class AppRole: IdentityRole<int>
    {
        public IList<AppUserRole> UserRoles { get; set; }
    }
}
