using Gym13.Application.Interfaces;
using Gym13.Application.Models;
using Gym13.Application.Models.Discount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym13.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class DiscountController : ControllerBase
    {
        readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet("list")]
        public async Task<List<DiscountModel>> GetDiscountList() => await _discountService.GetDiscountList();

        [HttpGet]
        public async Task<DiscountModel?> GetDiscount(int id) => await _discountService.GetDiscount(id);

        [HttpPost]
        public async Task<BaseResponseModel> CreateDiscount(DiscountModel request)
            => await _discountService.CreateDiscount(request);

        [HttpPut]
        public async Task<BaseResponseModel> UpdateDiscount(DiscountModel request)
            => await _discountService.UpdateDiscount(request);

        [HttpPatch("active-status")]
        public async Task<BaseResponseModel> ChangeDiscountActiveStatus(int id)
            => await _discountService.ChangeDiscountActiveStatus(id);

        [HttpDelete]
        public async Task DeleteDiscount(int id) => await _discountService.DeleteDiscount(id);
    }
}
