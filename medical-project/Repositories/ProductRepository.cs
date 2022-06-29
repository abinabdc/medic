using AutoMapper;
using AutoMapper.QueryableExtensions;
using medical_project.Dtos;
using medical_project.Interfaces;
using medical_project.Models;
using Microsoft.EntityFrameworkCore;

namespace medical_project.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
        {
            return await _context.Product.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToListAsync(); 
        }

        public async Task<ProductDto> GetAllProductByIdAsync(int Id)
        {
            return await _context.Product.Where(p=>p.Id == Id).ProjectTo<ProductDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ProductDto>> GetProductByStoreId(int Id)
        {
            return await _context.Product.Where(p => p.PharmacyId == Id).ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<bool> ProductExists(string name)
        {
            return await _context.Product.AnyAsync(p => p.Name == name);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}
