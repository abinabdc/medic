﻿using AutoMapper;
using medical_project.Dtos;
using medical_project.Extensions;
using medical_project.Interfaces;
using medical_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public OrderController(IOrderRepository orderRepo, IMapper mapper, DataContext ctx, IPharmacyRepository pharmacyRepo, IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            _context = ctx;
            _pharmacyRepo = pharmacyRepo;
            _productRepo = productRepo;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostOrder(OrderDto dto)
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var product = await _productRepo.GetAllProductByIdAsync(dto.ProductId);
            if (product == null)
            {
                var msg = new { Message = "No product found" };
                return BadRequest(msg);
            }
            var newOrder = new Order
            {
                AppUserId = userId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                TotalPrice = dto.TotalPrice,
                Status = "Processing",
                PaymentType = dto.PaymentType,
                PaymentStatus = "Not Paid"
            };
           await _context.Order.AddAsync(newOrder);
            if (await _orderRepo.SaveAllAsync())
            {
                var alr = new { Message = "Order Placed Successfully" };
                return Ok(alr);
            }
            return BadRequest("Something went wrong");
            
            


        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var result  = await _orderRepo.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("my-shop")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyShopOrders()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var userPharmacy = await _pharmacyRepo.GetPharmacyByUserId(userId);
            if (userPharmacy == null)
            {
                var msg = new { Message = "Pharmacy with the given ID couldnot be found" };
                return NotFound(msg);
            }
            var orders = await _orderRepo.GetAllByPharmacyIdAsync(userPharmacy.Id);
            return Ok(orders);
        }
        [HttpGet("my-shop-completed")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyShopCompletedOrders()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var userPharmacy = await _pharmacyRepo.GetPharmacyByUserId(userId);
            if (userPharmacy == null)
            {
                var msg = new { Message = "Pharmacy with the given ID couldnot be found" };
                return NotFound(msg);
            }
            var orders = await _orderRepo.GetCompletedByPharmacy(userPharmacy.Id);
            return Ok(orders);
        }
        [HttpGet("my-shop-pending")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyShopPendingOrders()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            var userPharmacy = await _pharmacyRepo.GetPharmacyByUserId(userId);
            if (userPharmacy == null)
            {
                var msg = new { Message = "Pharmacy with the given ID couldnot be found" };
                return NotFound(msg);
            }
            var orders = await _orderRepo.GetPendingByPharmacyIdAsync(userPharmacy.Id);
            return Ok(orders);
        }

        [HttpGet("my-orders")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrder()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            if (username == null)
            {
                var msg = new { Message = "Not a valid user." };
                return NotFound(msg);
            }
            var orders = await _orderRepo.GetAllByUserId(userId);
            return Ok(orders);
        }
        [HttpGet("my-completed-orders")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserCompletedOrder()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            if (username == null)
            {
                var msg = new { Message = "Not a valid user." };
                return NotFound(msg);
            }
            var orders = await _orderRepo.GetCompletedByUserId(userId);
            return Ok(orders);
        }
        [HttpGet("my-pending-orders")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserPendingOrder()
        {
            var username = User.GetUsername();
            var userId = Int32.Parse(username);
            if (username == null)
            {
                var msg = new { Message = "Not a valid user." };
                return NotFound(msg);
            }
            var orders = await _orderRepo.GetPendingByUserId(userId);
            return Ok(orders);
        }




    }
}