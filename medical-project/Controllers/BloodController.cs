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
using Microsoft.EntityFrameworkCore;

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
                    AppUserId = userId,
                    
                };
                _context.BloodRequest.Add(bloodrq);
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
                    _context.BloodRequest.Remove(await _bloodRepo.GetBloodRequestById(deleteRequestDto.ReqId));
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
        [HttpPost("donating")]
        public async Task<ActionResult> Donating(UserDonatingBloodDto donatingDto)
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var BloodPost = await _bloodRepo.GetBloodRequestById(donatingDto.BloodRequestId);
            if (BloodPost == null)
            {
                return BadRequest("Invalid blood request");
            }
            if (await _bloodRepo.UsersAlreadyGoing(userId, donatingDto.BloodRequestId))
            {
                return BadRequest("The User is already going");
            }
            if (donatingDto.MLDonating > 470)
            {
                return BadRequest("A average human can only donate upto 470ML");
            }
            if (BloodPost.RequiredML < BloodPost.ReceivedML + donatingDto.MLDonating)
            {
                return BadRequest("Exceeding amount detected");
            }
            if (await _bloodRepo.GetBloodRequestById(donatingDto.BloodRequestId) != null)
            {
                var going = new UserDonatingBlood
                {
                    AppUserId = userId,
                    /*UserDonating = await _userRepo.GetUserByIdInternalUse(userId),*/
                    BloodRequestId = donatingDto.BloodRequestId,
                    /*BloodRequestPost = await _bloodRepo.GetBloodRequestById(reqId),*/
                    MLDonating = donatingDto.MLDonating,
                };
                _context.UsersDonating.Add(going);
                if (await _context.SaveChangesAsync() > 0)
                {
                    
                    
                    BloodPost.ReceivedML = BloodPost.ReceivedML + donatingDto.MLDonating;
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return Ok("Everything went well");
                    }
                    else
                    {
                        _context.UsersDonating.Remove(going);
                        return BadRequest("Something went wrongg");
                    }
                }
            }
            return BadRequest("Nothing worked");

        }
        [HttpPut("donating")]
        public async Task<IActionResult> EditDonatingAmount(UserDonatingBloodDto donatingDto)
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var BloodPost = await _bloodRepo.GetBloodRequestById(donatingDto.BloodRequestId);
            var RequestBeingEdited = await _bloodRepo.UserDonatingBlood(userId, donatingDto.BloodRequestId);
            if (BloodPost == null)
            {
                return BadRequest("Invalid blood request");
            }
            if (!await _bloodRepo.UsersAlreadyGoing(userId, donatingDto.BloodRequestId))
            {
                return BadRequest("The User is not going");
            }
            if (donatingDto.MLDonating > 470)
            {
                return BadRequest("A average human can only donate upto 470ML");
            }
            if (BloodPost.RequiredML < BloodPost.ReceivedML + donatingDto.MLDonating)
            {
                return BadRequest("Exceeding amount detected");
            }

            RequestBeingEdited.MLDonating = donatingDto.MLDonating;
            if (await _bloodRepo.SaveAllAsync())
            {
                return Ok("Edited Sucessfully");
            }
            return BadRequest("Something went wrong");
        }
        [HttpDelete("donating/{id}")]
        public async Task<IActionResult> UndoDonation(int id)
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var BloodPost = await _bloodRepo.GetBloodRequestById(id);
            var RequestBeingEdited = await _bloodRepo.UserDonatingBlood(userId, id);

            if (BloodPost == null)
            {
               return NotFound("Request Not Found");
            }
            if (RequestBeingEdited == null)
            {
                return BadRequest("You are not donating.");
            }
            _context.UsersDonating.Remove(RequestBeingEdited);
            if (await _bloodRepo.SaveAllAsync())
            {
                return BadRequest("You are not donating anymore.");
            }
            return BadRequest("Something went wrong while processing you request");
            


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
