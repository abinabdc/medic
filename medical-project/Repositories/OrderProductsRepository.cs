using AutoMapper;
using AutoMapper.QueryableExtensions;
using medical_project.Dtos;
using medical_project.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace medical_project.Repositories
{
    public class OrderProductsRepository : IOrderProductsRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public OrderProductsRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<IEnumerable<OrderProductDto>> GetAllOrdersFromPharmacyId(int pharmacyId)
        {
            return await _context.OrderProducts.Where(p => p.Product.PharmacyId == pharmacyId).ProjectTo<OrderProductDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<IEnumerable<OrderProductDto>> GetAllCompletedOrdersFromPharmacyId(int pharmacyId)
        {
            return await _context.OrderProducts.Where(p => p.Product.PharmacyId == pharmacyId && p.Order.OrderStatus == "Delivered").ProjectTo<OrderProductDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<IEnumerable<OrderProductDto>> GetAllPendingOrdersFromPharmacyId(int pharmacyId)
        {
            return await _context.OrderProducts.Where(p => p.Product.PharmacyId == pharmacyId && p.Order.OrderStatus != "Delivered").ProjectTo<OrderProductDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<bool> VerifyOrderIdFromPharmacyId(int orderId, int pharmacyId)
        {
            return await _context.OrderProducts.Where(p => p.OrderId == orderId && p.Product.PharmacyId == pharmacyId).AnyAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(ResponseOrderDto dto)
        {
            _context.Entry(dto).State = EntityState.Modified;
        }
    }
}
