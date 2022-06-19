using AutoMapper;
using AutoMapper.QueryableExtensions;
using medical_project.Dtos;
using medical_project.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace medical_project.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;

        }

        public async Task<UserDetailDto> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Where(x => x.Id == id)
                .ProjectTo<UserDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByIdInternalUse(int id)
        {
            return await _context.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<UserDetailDto> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
               .Where(x => x.UserName == username)
               .ProjectTo<UserDetailDto>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserDetailDto>> GetUsersAsync()
        {
            return await _context.Users
                .ProjectTo<UserDetailDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

       
    }
}
