using medical_project.Models;

namespace medical_project.Interfaces
{
    public interface IRequestBloodRepository
    {
        void Update(BloodRequest bloodRequest);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<BloodRequest>> GetBloodRequestsAsync();
        Task<BloodRequest> GetBloodRequestById(int id);
        Task<IEnumerable<BloodRequest>> GetBloodRequestWithGroup(string bloodType);
        Task<IEnumerable<BloodRequest>> GetBloodRequestsWithExpiry(bool isExpired);

    }
}
