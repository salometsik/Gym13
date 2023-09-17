using Gym13.Application.Models.InfoTab;
using Gym13.Application.Models;

namespace Gym13.Application.Interfaces
{
    public interface IInfoTabService
    {
        #region Manage
        Task<BaseResponseModel> AddInfoTab(InfoTabModel request);
        Task<InfoTabModel?> GetInfoTab(int id);
        Task<BaseResponseModel> UpdateInfoTab(InfoTabModel request);
        Task<BaseResponseModel> DeleteInfoTab(int id);
        #endregion
        Task<List<InfoTabModel>> GetInfoTabList(int? pageSize);
    }
}
