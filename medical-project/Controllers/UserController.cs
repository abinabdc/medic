using AutoMapper;
using medical_project.Dtos;
using medical_project.Extensions;
using medical_project.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace medical_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;


        public UserController(UserManager<AppUser> userManager, IUserRepository userRepository, DataContext ctx, IMapper mapper, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _userRepo = userRepository;
            _mapper = mapper;
            _roleManager = roleManager;
            _context = ctx;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsers()
        {
            var users = await _userRepo.GetUsersAsync();
            return Ok(CustomResponse.CustResponse(users, true));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUserDto>> GetUser(int id)
        {
            var result = await _userRepo.GetUserByIdAsync(id);
            return Ok(CustomResponse.CustResponse(result, true));
        }
        [HttpGet("my-details")]
        [Authorize]
        public async Task<ActionResult<AppUserDto>> GetDetails()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var result = await _userRepo.GetUserByIdAsync(userId);
            
            return Ok(CustomResponse.CustResponse(result, true));
        }
    }
}
