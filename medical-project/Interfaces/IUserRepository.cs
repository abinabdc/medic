using medical_project.Dtos;

namespace medical_project.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<UserDetailDto>> GetUsersAsync();
        Task<UserDetailDto> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByIdInternalUse(int id);
        Task<UserDetailDto> GetUserByUsernameAsync(string username);
    }
}
