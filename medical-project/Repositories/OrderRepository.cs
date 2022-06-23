using AutoMapper;
using AutoMapper.QueryableExtensions;
using medical_project.Dtos;
using medical_project.Interfaces;
using medical_project.Models;
using Microsoft.EntityFrameworkCore;

namespace medical_project.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public OrderRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<IEnumerable<ResponseOrderDto>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Order.Where(p => p.AppUserId == userId).ProjectTo<ResponseOrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<ResponseOrderDto>> GetAllCompletedOrdersAsync()
        {
            return await _context.Order.Where(p => p.OrderStatus == "Delivered").ProjectTo<ResponseOrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<ResponseOrderDto>> GetAllOrders()
        {
            return await _context.Order.ProjectTo<ResponseOrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<ResponseOrderDto>> GetAllPendingOrdersAsync()
        {
            return await _context.Order.Where(p => p.OrderStatus != "Delivered").ProjectTo<ResponseOrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<ResponseOrderDto>> GetCompleByUserIdAsync(int userId)
        {
            return await _context.Order.Where(p => p.AppUserId == userId && p.OrderStatus == "Delivered").ProjectTo<ResponseOrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<ResponseOrderDto> GetOrderById(int id)
        {
            return await _context.Order.Where(p => p.OrderId == id).ProjectTo<ResponseOrderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<Order> GetOrderByIdInternalUse(int id)
        {
            return await _context.Order.Where(p => p.OrderId == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ResponseOrderDto>> GetPendingByUserIdAsync(int userId)
        {
            return await _context.Order.Where(p => p.AppUserId == userId && p.OrderStatus != "Delivered").ProjectTo<ResponseOrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
        }

    }
}
