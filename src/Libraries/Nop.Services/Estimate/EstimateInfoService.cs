using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Estimate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Services.Estimate
{
    public class EstimateInfoService : IEstimateInfoService
    {
        private IRepository<EstimateInfo> _estimateInfoRepository;

        public EstimateInfoService(IRepository<EstimateInfo> estimateInfoRepository)
        {
            _estimateInfoRepository = estimateInfoRepository;
        }

        public IList<EstimateInfo> GetAllEstimateInfos()
        {
            return _estimateInfoRepository.Table.ToList();
        }

        public IList<EstimateInfo> GetNewEstimateInfos()
        {
            return _estimateInfoRepository.Table.Where(_ => _.IsFinish == false).ToList();
        }
        public IPagedList<EstimateInfo> GetAllEstimateInfosByPage(int pageIndex = 0, int pageSize = Int32.MaxValue, bool showHidden = false)
        {
            throw new NotImplementedException();
        }

        public void InsertEstimateInfo(EstimateInfo estimateInfo)
        {
            _estimateInfoRepository.Insert(estimateInfo);
        }

        public void UpdateEstimateInfo(EstimateInfo estimateInfo)
        {
            _estimateInfoRepository.Update(estimateInfo);
        }

        public EstimateInfo GetEstimateInfoById(int id)
        {
            if (id == 0)
                return null;

            return _estimateInfoRepository.GetById(id);
        }

        public EstimateInfo GetUnFinishEstimateInfoByCustomerId(int customerId)
        {
            if (customerId == 0)
                return null;

            return _estimateInfoRepository.Table.LastOrDefault(_ => _.CustomerId == customerId && _.IsFinish == false);
        }

        public void DeleteEstimateInfo(EstimateInfo estimateInfo)
        {
            if (estimateInfo == null)
                throw new ArgumentNullException(nameof(estimateInfo));

            _estimateInfoRepository.Delete(estimateInfo);
        }
    }
}
