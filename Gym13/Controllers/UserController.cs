using Gym13.Application.Interfaces;
using Gym13.Persistence.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gym13.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IGymService _gymService;

        public UserController(IGymService gymService)
        {
            _gymService = gymService;
        }

        //[HttpGet]
        //public async Task<ApplicationUser?> GetUser(string userId) => await _gymService.GetUser(userId);
    }
}
