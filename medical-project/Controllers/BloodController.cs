using AutoMapper;
using medical_project.Dtos;
using medical_project.Extensions;
using medical_project.Interfaces;
using medical_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;

namespace medical_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IRequestBloodRepository _bloodRepo;
        private readonly IUserRepository _userRepo;
        private readonly UserManager<AppUser> _userManager;

        public BloodController(UserManager<AppUser> userManager, DataContext ctx, IMapper mapper, IRequestBloodRepository bloodRepo, IUserRepository userRepo)
        {
            _context = ctx;
            _mapper = mapper;
            _bloodRepo = bloodRepo;
            _userRepo = userRepo;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BloodRequestDto>>> GetBloodRequests()
        {
            var result = await _bloodRepo.GetBloodRequestsWithExpiry(false);
            return Ok(result);
            /*return Ok(result.ProjectTo<BloodRequestDto>(_mapper.ConfigurationProvider));*/
        }
        [HttpPost("req-new")]
        [Authorize(Policy = "Normal")]
        public async Task<ActionResult> RequestBlood(BloodRequestDto bloodReqDto)
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var userRequesting = await _userRepo.GetUserByIdAsync(userId);
            if (userRequesting != null)
            {
                var bloodrq = new BloodRequest
                {
                    BloodGroup = bloodReqDto.BloodGroup,
                    isExpired = false,
                    RequiredML = bloodReqDto.RequiredML,
                    ReceivedML = bloodReqDto.ReceivedML,
                    ExtraComments = bloodReqDto.ExtraComments,
                    Location = bloodReqDto.Location,
                    AppUserId = userId
                };
                _context.BloodRequests.Add(bloodrq);
                if (await _bloodRepo.SaveAllAsync())
                {
                    return Ok("Requested Successfully");
                }
                else
                {
                    return BadRequest("Something went wrong while saving the request");
                }
            }
            return BadRequest("User with userID doesnot exists");
        }
        [HttpDelete("delete-req")]
        [Authorize]
        public async Task<ActionResult> DeleteRequest(DeleteRequestDto deleteRequestDto)
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var userRequesting = await _userRepo.GetUserByIdInternalUse(userId);

            if (userRequesting != null)
            {
               if (await IsPasswordCorrect(userRequesting, deleteRequestDto.Password))
                {
                    _context.BloodRequests.Remove(await _bloodRepo.GetBloodRequestById(deleteRequestDto.ReqId));
                    if (await _bloodRepo.SaveAllAsync())
                    {
                        return Ok("The Request was deleted successfully");
                    }
                }
                else
                {
                    return Ok("The password is not correct");
                }
            }
            return BadRequest("There was no matching user");

            
        }

        private async Task<bool> IsPasswordCorrect(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        public class DeleteRequestDto
        {
            public int ReqId { get; set; }
            public string Password { get; set; }
        }


    }
}
