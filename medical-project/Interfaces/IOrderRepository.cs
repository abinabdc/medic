using medical_project.Dtos;
using medical_project.Models;

namespace medical_project.Interfaces
{
    public interface IOrderRepository
    {
        void Update(Order order);
        Task<bool> SaveAllAsync();
        Task<ResponseOrderDto> GetOrderById(int id);
        Task<Order> GetOrderByIdInternalUse(int id);

        Task<IEnumerable<ResponseOrderDto>> GetAllByUserIdAsync(int userId);
        Task<IEnumerable<ResponseOrderDto>> GetPendingByUserIdAsync(int userId);
        Task<IEnumerable<ResponseOrderDto>> GetCompleByUserIdAsync(int userId);

        //For admin
        Task<IEnumerable<ResponseOrderDto>> GetAllOrders();
        Task<IEnumerable<ResponseOrderDto>> GetAllPendingOrdersAsync();
        Task<IEnumerable<ResponseOrderDto>> GetAllCompletedOrdersAsync();

        /* Task<IEnumerable<OrderDto>> GetAllAsync();
         Task<IEnumerable<OrderDto>> GetAllByUserId(int Id);
         Task<IEnumerable<OrderDto>> GetPendingByUserId(int userId);
         Task<IEnumerable<OrderDto>> GetAllByPharmacyIdAsync(int Id);
         Task<IEnumerable<OrderDto>> GetPendingByPharmacyIdAsync(int Id);
         Task<IEnumerable<OrderDto>> GetCompletedByUserId(int Id);
         Task<IEnumerable<OrderDto>> GetCompletedByPharmacy(int Id);*/
    }
}
