using medical_project.Dtos;
using medical_project.Extensions;
using medical_project.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medical_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly DataContext _context;
        private readonly RoleManager<AppRole> _roleManager;

        public AccountController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, DataContext ctx)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = ctx;
            _roleManager = roleManager;
        }
        [HttpPost("register")]
        public async Task<ActionResult<LoginDto>> Register(AppUserDto appUser)
        {
            if (await UserExists(appUser.UserName)) return BadRequest(CustomResponse.CustResponse($"Username with {appUser.UserName} already exists", false));
            if (await EmailExits(appUser.Email)) return BadRequest(CustomResponse.CustResponse($"Username with {appUser.Email} already exists", false));
            var user = new AppUser
            {
                UserName = appUser.UserName.ToLower(),
                Email = appUser.Email,
                FullName = appUser.FullName,
                BloodGroup = appUser.BloodGroup,
                Weight = appUser.Weight,
                DOB = appUser.DOB,
                Height = appUser.Height,
                PhoneNumber = appUser.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, appUser.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            var roleResult = await _userManager.AddToRoleAsync(user, "Normal");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);
            var finalResult = new LoginDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Roles = user.UserRoles.Select(r => r.Role.Name).ToList()
            };
            return Ok(CustomResponse.CustResponse(finalResult, true));

        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginDto>> Login(LoginDetails loginDetails)
        {

            var user = await _userManager.Users.Include(r => r.UserRoles).ThenInclude(r => r.Role).OrderBy(u => u.UserName).SingleOrDefaultAsync(x => x.UserName == loginDetails.Username.ToLower());
            if (user == null) return NotFound(CustomResponse.CustResponse($"User with {loginDetails.Username} username doesnot exists.", false));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDetails.Password, false);
            if (!result.Succeeded) return Unauthorized(CustomResponse.CustResponse("Username or Password didn't match", false));

            var finalResult = new LoginDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Roles = user.UserRoles.Select(r => r.Role.Name).ToList()
            };
            return Ok(CustomResponse.CustResponse(finalResult, true));




        }
        
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        private async Task<bool> EmailExits(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
        public class LoginDetails
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }


    }
}
