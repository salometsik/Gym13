using Gym13.Application.Interfaces;
using Gym13.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym13.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountController : ControllerBase
    {
        readonly IPlanService _gymService;

        public AccountController(IPlanService gymService)
        {
            _gymService = gymService;
        }

        //[HttpGet]
        //public async Task<ApplicationUser?> GetUser(string userId) => await _gymService.GetUser(userId);
    }
}
