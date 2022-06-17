﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using medical_project.Dtos;
using medical_project.Interfaces;
using medical_project.Models;
using Microsoft.EntityFrameworkCore;

namespace medical_project.Repositories
{
    public class RequestBloodRepository : IRequestBloodRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public RequestBloodRepository(DataContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;

        }

        public async Task<BloodRequest> GetBloodRequestById(int id)
        {
            return await _context.BloodRequests.Where(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BloodRequest>> GetBloodRequestsAsync()
        {
            return await _context.BloodRequests.ToListAsync();
        }

        public async Task<IEnumerable<BloodRequestDto>> GetBloodRequestsWithExpiry(bool isExpired)
        {
            return await _context.BloodRequests.Where(b => b.isExpired == isExpired).ProjectTo<BloodRequestDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<BloodRequest>> GetBloodRequestWithGroup(string bloodType)
        {
            return await _context.BloodRequests.Where(b => b.BloodGroup == bloodType).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(BloodRequest bloodRequest)
        {
            _context.Entry(bloodRequest).State = EntityState.Modified;
        }
    }
}
