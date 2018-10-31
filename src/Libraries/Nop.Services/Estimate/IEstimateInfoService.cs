using Nop.Core;
using Nop.Core.Domain.Estimate;
using System.Collections.Generic;

namespace Nop.Services.Estimate
{
    public interface IEstimateInfoService
    {
        IList<EstimateInfo> GetAllEstimateInfos();
        IList<EstimateInfo> GetNewEstimateInfos();
        IPagedList<EstimateInfo> GetAllEstimateInfosByPage(int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        void InsertEstimateInfo(EstimateInfo estimateInfo);
        void UpdateEstimateInfo(EstimateInfo estimateInfo);
        EstimateInfo GetEstimateInfoById(int id);
        EstimateInfo GetUnFinishEstimateInfoByCustomerId(int customerId);

        void DeleteEstimateInfo(EstimateInfo estimateInfo);

    }
}
