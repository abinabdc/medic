using medical_project.Dtos;
using medical_project.Extensions;
using medical_project.Interfaces;
using medical_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace medical_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacyController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IPharmacyRepository _pharmacyRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepo;
        

        public PharmacyController(IUserRepository userRepo, UserManager<AppUser> userManager, DataContext ctx, IPharmacyRepository pharmacyRepository)
        {
            _context = ctx;
            _pharmacyRepository = pharmacyRepository;
            _userManager = userManager;
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PharmacyDto>>> GetPharmacies()
        {
            var result = await _pharmacyRepository.GetPharmaciesAsync();
            return Ok(CustomResponse.CustResponse("Pharmacies has been fetched successfully", true, result));
        }
        [HttpGet("my-pharmacy")]
        public async Task<ActionResult<PharmacyDto>> GetMyPharmacy()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);

            var result = await _pharmacyRepository.GetPharmacyByUserId(userId);
            if (result == null)
            {
                return BadRequest(CustomResponse.CustResponse("The user has no Pharmacy registered.", false));
            }
            else
            {
                return Ok(CustomResponse.CustResponse("Pharmacy has been fetched successfully", true, result));
            }

        }
        [HttpPost]
        public async Task<ActionResult> PostPharmacy(PharmacyDto dto)
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);

            if (await _pharmacyRepository.PharmacyExists(userId))
            {
                return BadRequest(CustomResponse.CustResponse("User has already one existing pharmacy.", false));
            }
            var newPharmacy = new Pharmacy
            {
                PharmacyName = dto.PharmacyName,
                Location = dto.Location,
                PANNo = dto.PANNo,
                OwnerQualification = dto.OwnerQualification,
                AppUserId = userId
            };
            _context.Pharmacy.Add(newPharmacy);
            if(await _pharmacyRepository.SaveAllAsync())
            {
                var user = await _userRepo.GetUserByIdInternalUse(userId);
                var roleResult = await _userManager.AddToRoleAsync(user, "Vendor");
                if (roleResult.Succeeded)
                {
                    return Ok(CustomResponse.CustResponse("New Pharmacy has been created sucessfully", true));
                    

                }
                else
                {
                   return BadRequest(CustomResponse.CustResponse("Something went wrong while assigning the vendor role.", false));
                }
            }
            return BadRequest(CustomResponse.CustResponse("Something went wrong.", false));
        }

    }
}
