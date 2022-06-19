using AutoMapper;
using AutoMapper.QueryableExtensions;
using medical_project.Dtos;
using medical_project.Interfaces;
using medical_project.Models;
using Microsoft.EntityFrameworkCore;

namespace medical_project.Repositories
{
    public class PharmacyRepository : IPharmacyRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public PharmacyRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PharmacyDto>> GetPharmaciesAsync()
        {
            return await _context.Pharmacy.ProjectTo<PharmacyDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<PharmacyDto>> GetPharmaciesByLocationAsync(string location)
        {
            return await _context.Pharmacy.Where(p => p.Location == location).ProjectTo<PharmacyDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<PharmacyDto> GetPharmacyByIdAsync(int id)
        {
            return await _context.Pharmacy.Where(p=>p.Id == id).ProjectTo<PharmacyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<Pharmacy> GetUserByIdInternalUse(int id)
        {
            return await _context.Pharmacy.Where(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<bool> PharmacyExists(int id)
        {
            return await _context.Pharmacy.AnyAsync(i => i.AppUserId == id);
        }
        public async Task<PharmacyDto> GetPharmacyByUserId(int id)
        {
            return await _context.Pharmacy.Where(p => p.AppUserId == id).ProjectTo<PharmacyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Pharmacy pharmacy)
        {
            _context.Entry(pharmacy).State = EntityState.Modified;
        }
    }
}
