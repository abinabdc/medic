using AutoMapper;
using medical_project.Dtos;
using medical_project.Models;

namespace medical_project.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<BloodRequest, BloodRequestDto>();
            CreateMap<AppUser, AppUserDto>();
            CreateMap<AppUser, UserDetailDto>();
            CreateMap<UserDonatingBlood, UserDonatingBloodDto>();
            CreateMap<Pharmacy, PharmacyDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderProducts, OrderProductDto>();
            CreateMap<Order, ResponseOrderDto>();
        }
    }
}

