using medical_project.Dtos;

namespace medical_project.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUserDto>> GetUsersAsync();
        Task<AppUserDto> GetUserByIdAsync(int id);
        Task<AppUserDto> GetUserByIdInternalUse(int id);
        Task<AppUserDto> GetUserByUsernameAsync(string username);
    }
}
