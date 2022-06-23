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
        [Authorize]
        public async Task<ActionResult<IEnumerable<BloodRequestDto>>> GetBloodRequests()
        {
            var result = await _bloodRepo.GetBloodRequestsWithExpiry(false);
            return Ok(CustomResponse.CustResponse(result, true));
            /*return Ok(result.ProjectTo<BloodRequestDto>(_mapper.ConfigurationProvider));*/
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BloodRequestDto>> GetBloodRequestById(int id)
        {
            var result = await _bloodRepo.GetBloodRequestById(id);
            return Ok(CustomResponse.CustResponse(result, true));
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
                    return Ok(CustomResponse.CustResponse("Blood request has been posted successfully.", true));
                }
                else
                {
                    return BadRequest(CustomResponse.CustResponse("Something went wrong while saving your request", false));
                }
            }
            return BadRequest(CustomResponse.CustResponse("Can not verify the user. Please logout and relogin if the issue persists", false));
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
                        return Ok(CustomResponse.CustResponse("The request was deleted successfuly", true));
                    }
                }
                else
                {
                    return BadRequest(CustomResponse.CustResponse("Incorrect Password", true));
                }
            }
            return BadRequest(CustomResponse.CustResponse("Can not verify the user. Please logout and relogin if the issue persists", false));
        }
        [HttpPost("donating")]
        public async Task<ActionResult> Donating(UserDonatingBloodDto donatingDto)
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var BloodPost = await _bloodRepo.GetBloodRequestById(donatingDto.BloodRequestId);
            if (BloodPost == null)
            {
                return BadRequest(CustomResponse.CustResponse("The Request to which you are trying to donate is not valid. Please re-check the request", false));
            }
            if (await _bloodRepo.UsersAlreadyGoing(userId, donatingDto.BloodRequestId))
            {
                return BadRequest(CustomResponse.CustResponse("You are already donating your blood for this request.", false));
            }
            if (donatingDto.MLDonating > 470)
            {
                return BadRequest(CustomResponse.CustResponse("A average human being can only donate upto 470ml.", false));
            }
            if (BloodPost.RequiredML < BloodPost.ReceivedML + donatingDto.MLDonating)
            {
                return BadRequest(CustomResponse.CustResponse("Exceeding amount detected.", false));
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
                        return BadRequest(CustomResponse.CustResponse($"You are donating {donatingDto.MLDonating}ml. Thank you for your blood donation.", true));
                    }
                    else
                    {
                        _context.UsersDonating.Remove(going);
                        return BadRequest(CustomResponse.CustResponse("Something went wrong.", false));
                    }
                }
            }
            return BadRequest(CustomResponse.CustResponse("Something went wrong.", false));

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
                return BadRequest(CustomResponse.CustResponse("The Request to which you are trying to donate is not valid. Please re-check the request", false));
            }
            if (!await _bloodRepo.UsersAlreadyGoing(userId, donatingDto.BloodRequestId))
            {
                return BadRequest(CustomResponse.CustResponse("You are not donating your blood for this request.", false));
            }
            if (donatingDto.MLDonating > 470)
            {
                return BadRequest(CustomResponse.CustResponse("A average human being can only donate upto 470ml.", false));
            }
            if (BloodPost.RequiredML < BloodPost.ReceivedML + donatingDto.MLDonating)
            {
                return BadRequest(CustomResponse.CustResponse("Exceeding amount detected.", false));
            }

            RequestBeingEdited.MLDonating = donatingDto.MLDonating;
            if (await _bloodRepo.SaveAllAsync())
            {
                return Ok(CustomResponse.CustResponse("The record has been updated successfully.", true));
            }
            return BadRequest(CustomResponse.CustResponse("Something went wrong.", false));
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
                return BadRequest(CustomResponse.CustResponse("The Request to which you are trying to perform action on is not valid. Please re-check the request", false));
            }
            if (RequestBeingEdited == null)
            {
                return BadRequest(CustomResponse.CustResponse("You are already not donating for this request.", false));
            }
            _context.UsersDonating.Remove(RequestBeingEdited);
            if (await _bloodRepo.SaveAllAsync())
            {
                return Ok(CustomResponse.CustResponse("You are not donating anymore.", false));
            }
            return BadRequest(CustomResponse.CustResponse("Something went wrong while processing your request.", false));



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
