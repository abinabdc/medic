using medical_project.Dtos;
using medical_project.Models;

namespace medical_project.Interfaces
{
    public interface IPharmacyRepository
    {
        void Update(Pharmacy pharmacy);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<PharmacyDto>> GetPharmaciesAsync();
        Task<PharmacyDto> GetPharmacyByIdAsync(int id);
        Task<PharmacyDto> GetPharmacyByUserId(int id);
        Task<Pharmacy> GetUserByIdInternalUse(int id);
        Task<bool> PharmacyExists(int id);
        Task<IEnumerable<PharmacyDto>> GetPharmaciesByLocationAsync(string location);
    }
}

