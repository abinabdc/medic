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
        }
    }
}
