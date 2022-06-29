using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace medical_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly RoleManager<AppRole> _roleManager;
        public RoleController(DataContext context, RoleManager<AppRole> role)
        {
            _context = context;
            _roleManager = role;


        }
        [HttpPost]
        public async Task<ActionResult> PostRoles()
        {
            var roles = new List<AppRole>
            {
                new AppRole{Name = "Normal"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Vendor"}
            };
            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(role);
            }
            _context.SaveChanges();
            return Ok("Added roles");
            
        }

    }
}
