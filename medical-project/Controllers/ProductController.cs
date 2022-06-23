using AutoMapper;
using medical_project.Dtos;
using medical_project.Extensions;
using medical_project.Interfaces;
using medical_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace medical_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPharmacyRepository _pharmacyRepo;

        public ProductController(IPharmacyRepository pharmacyRepo, UserManager<AppUser> userManager, DataContext ctx, IMapper mapper, IProductRepository productRepository, IUserRepository userRepo)
        {
            _context = ctx;
            _mapper = mapper;
            _productRepository = productRepository;
            _userRepo = userRepo;
            _userManager = userManager;
            _pharmacyRepo = pharmacyRepo;
        }
        [HttpGet("my-products")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetMyProducts()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var userPharmacy = await _pharmacyRepo.GetPharmacyByUserId(userId);
            if (userPharmacy == null)
            {
                return BadRequest(CustomResponse.CustResponse("The pharmacy, you are trying to post from doest not exists", false));
            }
            var result = await _productRepository.GetProductByStoreId(userPharmacy.Id);
            return Ok(CustomResponse.CustResponse(result, true));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var result = await _productRepository.GetAllProductAsync();
            return Ok(CustomResponse.CustResponse(result, true));
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddProduct(ProductDto dto)
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);

            var userPharmacy = await _pharmacyRepo.GetPharmacyByUserId(userId);
            if (userPharmacy == null)
            {
                return BadRequest(CustomResponse.CustResponse("You do not have any pharmacy registered", false));
            }

            if (await _productRepository.ProductExists(dto.Name))
            {
                return BadRequest(CustomResponse.CustResponse("Product with the same name already exists.", false));
            }
            else
            {
                var newProduct = new Product
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Dose = dto.Dose,
                    ApplicableFor = dto.ApplicableFor,
                    Description = dto.Description,
                    PharmacyId = userPharmacy.Id,
                };
                await _context.Product.AddAsync(newProduct);
                if (await _productRepository.SaveAllAsync())
                {
                    return Ok(CustomResponse.CustResponse("The Product has been posted successfully", true));
                }
                else
                {
                    return BadRequest(CustomResponse.CustResponse("Something went wrong.", false));
                }
            }

        }
        

    }
}
