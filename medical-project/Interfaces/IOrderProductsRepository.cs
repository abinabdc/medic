using medical_project.Dtos;

namespace medical_project.Interfaces
{
    public interface IOrderProductsRepository
    {
        /*void Update(OrderDto order);*/
        Task<bool> SaveAllAsync();
        Task<IEnumerable<OrderProductDto>> GetAllOrdersFromPharmacyId(int pharmacyId);
        Task<IEnumerable<OrderProductDto>> GetAllCompletedOrdersFromPharmacyId(int pharmacyId);
        Task<IEnumerable<OrderProductDto>> GetAllPendingOrdersFromPharmacyId(int pharmacyId);
        Task<bool> VerifyOrderIdFromPharmacyId(int orderId, int pharmacyId);
        void Update(ResponseOrderDto dto);


    }
}
