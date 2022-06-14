using AutoMapper;
using medical_project.Interfaces;
using medical_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace medical_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IRequestBloodRepository _bloodRepo;
        
        public BloodController(DataContext ctx, IMapper mapper, IRequestBloodRepository bloodRepo)
        {
            _context = ctx;
            _mapper = mapper;
            _bloodRepo = bloodRepo;
        }


    }
}
