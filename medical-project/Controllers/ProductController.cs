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
                var msg = new { Message = "Pharmacy doesnot exists" };
                return BadRequest(msg);
            }
            var result = await _productRepository.GetProductByStoreId(userPharmacy.Id);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var result = await _productRepository.GetAllProductAsync();
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductDto dto)
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);

            var userPharmacy = await _pharmacyRepo.GetPharmacyByUserId(userId);
            if (userPharmacy == null)
            {
                var msg = new { Message = "User doesnot have registered his pharmacy" };
                return BadRequest(msg);
            }

            if (await _productRepository.ProductExists(dto.Name))
            {
                var err = new { Message = "Product with the name already exists." };
                return BadRequest(err);
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
                    /*var msgg = new { Message = "Product has been posted successfully" };*/
                    return Ok();

                }
                else
                {
                    var error = new { Message = "Something went wrong" };
                    return BadRequest(error);
                }
            }

        }
        

    }
}
