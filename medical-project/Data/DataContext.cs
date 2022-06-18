
using medical_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace medical_project
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();
            /* builder.Entity<BloodRequest>().HasMany(d => d.Donors).*/

            builder.Entity<UserDonatingBlood>()
                .HasKey(u => new
                {
                    u.AppUserId,
                    u.BloodRequestId
                });









            /* builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();*/


        }
        public DbSet<BloodRequest> BloodRequest { get; set; }
        public DbSet<UserDonatingBlood> UsersDonating { get; set; }

    }
}
