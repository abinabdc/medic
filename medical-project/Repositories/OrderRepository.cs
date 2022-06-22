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
        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            return await _context.Order.ProjectTo<OrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetAllByPharmacyIdAsync(int Id)
        {
            return await _context.Order.Include(o => o.Product).Where(p => p.Product.PharmacyId == Id).ProjectTo<OrderDto>(_mapper.ConfigurationProvider).ToListAsync();
            /*return await _context.Order.Include(o => o.Product).Where(p => p.Product.PharmacyId == Id).ProjectTo<OrderDto>(_mapper.ConfigurationProvider).ToListAsync();*/
        }

        public async Task<IEnumerable<OrderDto>> GetAllByUserId(int Id)
        {
            return await _context.Order.Where(p => p.AppUserId == Id).ProjectTo<OrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetCompletedByPharmacy(int Id)
        {
            return await _context.Order.Include(o => o.Product).Where(p => p.Product.PharmacyId == Id && p.Status == "Delivered").ProjectTo<OrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetCompletedByUserId(int Id)
        {
            return await _context.Order.Include(o => o.Product).Where(p => p.AppUserId == Id && p.Status == "Delivered").ProjectTo<OrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetPendingByPharmacyIdAsync(int Id)
        {
           return await _context.Order.Include(o => o.Product).Where(p => p.Product.PharmacyId == Id && p.Status != "Delivered").ProjectTo<OrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetPendingByUserId(int userId)
        {
            return await _context.Order.Include(o => o.Product).Where(p => p.AppUserId == userId && p.Status != "Delivered").ProjectTo<OrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(OrderDto order)
        {
            _context.Entry(order).State = EntityState.Modified;
        }
    }
}
