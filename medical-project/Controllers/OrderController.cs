using AutoMapper;
using medical_project.Dtos;
using medical_project.Extensions;
using medical_project.Interfaces;
using medical_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medical_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IPharmacyRepository _pharmacyRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUserRepository _userRepo;
        private readonly IOrderProductsRepository _orderProRepo;

        public OrderController(IOrderProductsRepository orderProRepo, IUserRepository userRepo, IOrderRepository orderRepo, IMapper mapper, DataContext ctx, IPharmacyRepository pharmacyRepo, IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            _context = ctx;
            _pharmacyRepo = pharmacyRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
            _orderProRepo = orderProRepo;
        }
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostOrder(OrderDto dto)
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var finalOrder = new Order
            {
                AppUserId = userId,
                TotalPrice = dto.TotalPrice,
                PayStatus = "Not Paid",
                PayType = "Cash on Delivery",
                OrderStatus = "Processing",
            };
            await _context.Order.AddAsync(finalOrder);
            if (await _context.SaveChangesAsync() > 0)
            {
                foreach (var data in dto.product){
                    var newOrder = new OrderProducts
                    {
                        OrderId = finalOrder.OrderId,
                        ProductId = data.product_id,
                        Quantity = data.quantity
                    };
                    await _context.OrderProducts.AddAsync(newOrder);
                }
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok(CustomResponse.CustResponse("New Orders have been places", true));
                }
                return BadRequest(CustomResponse.CustResponse("Something went wrong", false));
                
            }
            return BadRequest(CustomResponse.CustResponse("Something went wrong", false));

        }
        [HttpGet("my-all-orders")]
        [Authorize]
        public async Task<ActionResult> GetAllMyOrders()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            if (await _userRepo.GetUserByIdInternalUse(userId) == null)
            {
                return BadRequest(CustomResponse.CustResponse("Can not verify the user.Please logout and relogin if the issue persists", false));
            }
            var result = await _orderRepo.GetAllByUserIdAsync(userId);
            return Ok(CustomResponse.CustResponse(result, true));
        }
        [HttpGet("my-pending-orders")]
        [Authorize]
        public async Task<ActionResult> GetMyPendingOrders()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            if (await _userRepo.GetUserByIdInternalUse(userId) == null)
            {
                return BadRequest(CustomResponse.CustResponse("Can not verify the user.Please logout and relogin if the issue persists", false));
            }
            var result = await _orderRepo.GetPendingByUserIdAsync(userId);
            return Ok(CustomResponse.CustResponse(result, true));
        }
        [HttpGet("my-completed-orders")]
        [Authorize]
        public async Task<ActionResult> GetMyCompletedOrders()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            if (await _userRepo.GetUserByIdInternalUse(userId) == null)
            {
                return BadRequest(CustomResponse.CustResponse("Can not verify the user.Please logout and relogin if the issue persists", false));
            }
            var result = await _orderRepo.GetCompleByUserIdAsync(userId);
            return Ok(CustomResponse.CustResponse(result, true));
        }
        [HttpGet("get-all-orders")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> GetAllOrders()
        {
            var result = await _orderRepo.GetAllOrders();
            return Ok(CustomResponse.CustResponse(result, true));
        }
        [HttpGet("get-pending-orders")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> GetAllPendingOrders()
        {
            var result = await _orderRepo.GetAllPendingOrdersAsync();
            return Ok(CustomResponse.CustResponse(result, true));
        }
        [HttpGet("get-completed-orders")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> GetCompletedOrders()
        {
            var result = await _orderRepo.GetAllCompletedOrdersAsync();
            return Ok(CustomResponse.CustResponse(result, true));
        }
        [HttpGet("get-mypharm-all-orders")]
        [Authorize(Policy = "VendorOnly")]
        public async Task<ActionResult> GetMyPharmacyOrders()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            
            if (await _userRepo.GetUserByIdInternalUse(userId) == null)
            {
                return BadRequest(CustomResponse.CustResponse("Can not verify the user.Please logout and relogin if the issue persists", false));
            }
            if (await _pharmacyRepo.GetPharmacyByUserId(userId) == null)
            {
                return BadRequest(CustomResponse.CustResponse("Can not find any pharmacy with the given Id.", false));
            }
            var pharm = await _pharmacyRepo.GetPharmacyByUserId(userId);
            var result = await _orderProRepo.GetAllOrdersFromPharmacyId(pharm.Id);
            return Ok(CustomResponse.CustResponse(result, true));
        }
        [HttpGet("get-mypharm-completed-orders")]
        [Authorize(Policy = "VendorOnly")]
        public async Task<ActionResult> GetMyPharmacyCompletedOrders()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);

            if (await _userRepo.GetUserByIdInternalUse(userId) == null)
            {
                return BadRequest(CustomResponse.CustResponse("Can not verify the user.Please logout and relogin if the issue persists", false));
            }
            if (await _pharmacyRepo.GetPharmacyByUserId(userId) == null)
            {
                return BadRequest(CustomResponse.CustResponse("Can not find any pharmacy with the given Id.", false));
            }
            var pharm = await _pharmacyRepo.GetPharmacyByUserId(userId);
            var result = await _orderProRepo.GetAllCompletedOrdersFromPharmacyId(pharm.Id);
            return Ok(CustomResponse.CustResponse(result, true));
        }
        [HttpGet("get-mypharm-pending-orders")]
        [Authorize(Policy = "VendorOnly")]
        public async Task<ActionResult> GetMyPharmacyPendingOrders()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);

            if (await _userRepo.GetUserByIdInternalUse(userId) == null)
            {
                return BadRequest(CustomResponse.CustResponse("Can not verify the user.Please logout and relogin if the issue persists", false));
            }
            if (await _pharmacyRepo.GetPharmacyByUserId(userId) == null)
            {
                return BadRequest(CustomResponse.CustResponse("Can not find any pharmacy with the given Id.", false));
            }
            var pharm = await _pharmacyRepo.GetPharmacyByUserId(userId);
            var result = await _orderProRepo.GetAllPendingOrdersFromPharmacyId(pharm.Id);
            return Ok(CustomResponse.CustResponse(result, true));
        }
        [HttpPut("edit-orders")]
        [Authorize(Policy = "VendorOnly")]
        public async Task<ActionResult> EditOrderDetails(EditOrderDto dto)
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);

            if (await _userRepo.GetUserByIdInternalUse(userId) == null)
            {
                return BadRequest(CustomResponse.CustResponse("Can not verify the user.Please logout and relogin if the issue persists", false));
            }
            if (await _pharmacyRepo.GetPharmacyByUserId(userId) == null)
            {
                return BadRequest(CustomResponse.CustResponse("Can not find any pharmacy with the given Id.", false));
            }
            var pharm = await _pharmacyRepo.GetPharmacyByUserId(userId);

            var orderBeingEdited = await _orderRepo.GetOrderByIdInternalUse(dto.OrderId);
            if (orderBeingEdited == null)
            {
                return BadRequest(CustomResponse.CustResponse("The Order you're editing is invalid, Please reload and try again.", false));
            }

            if (await _orderProRepo.VerifyOrderIdFromPharmacyId(dto.OrderId, pharm.Id))
            {
                orderBeingEdited.OrderStatus = dto.OrderStatus;
                orderBeingEdited.PayStatus = dto.PayStatus;

                _orderRepo.Update(orderBeingEdited);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok(CustomResponse.CustResponse("The Order has been edited", true));
                }
                return BadRequest(CustomResponse.CustResponse("Something went wrong while saving the changes", false));
            }
            return BadRequest(CustomResponse.CustResponse("Not an order for your pharmacy to modify", false));


        }

        public class EditOrderDto
        {
            public int OrderId { get; set; }
            public string OrderStatus { get; set; }
            public string PayStatus { get; set; }
        }









    }
}
