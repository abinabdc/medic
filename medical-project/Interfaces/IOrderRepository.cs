using medical_project.Dtos;
using medical_project.Models;

namespace medical_project.Interfaces
{
    public interface IOrderRepository
    {
        void Update(OrderDto order);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<IEnumerable<OrderDto>> GetAllByUserId(int Id);
        Task<IEnumerable<OrderDto>> GetPendingByUserId(int userId);
        Task<IEnumerable<OrderDto>> GetAllByPharmacyIdAsync(int Id);
        Task<IEnumerable<OrderDto>> GetPendingByPharmacyIdAsync(int Id);
        Task<IEnumerable<OrderDto>> GetCompletedByUserId(int Id);
        Task<IEnumerable<OrderDto>> GetCompletedByPharmacy(int Id);


    }
}
