using Gym13.Application.Interfaces;
using Gym13.Application.Models;
using Gym13.Application.Models.InfoTab;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym13.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class InfoTabController : ControllerBase
    {
        readonly IInfoTabService _infoTabService;

        public InfoTabController(IInfoTabService infoTabService)
        {
            _infoTabService = infoTabService;
        }

        [HttpGet("list")]
        public async Task<List<InfoTabModel>> GetInfoTabList(int? pageSize) => await _infoTabService.GetInfoTabList(pageSize);

        [HttpPost]
        public async Task<BaseResponseModel> AddInfoTab(InfoTabModel request) => await _infoTabService.AddInfoTab(request);

        [HttpGet]
        public async Task<InfoTabModel?> GetInfoTab(int id) => await _infoTabService.GetInfoTab(id);

        [HttpPut]
        public async Task<BaseResponseModel> UpdateInfoTab(InfoTabModel request)
            => await _infoTabService.UpdateInfoTab(request);

        [HttpDelete]
        public async Task<BaseResponseModel> DeleteInfoTab(int id) => await _infoTabService.DeleteInfoTab(id);
    }
}
