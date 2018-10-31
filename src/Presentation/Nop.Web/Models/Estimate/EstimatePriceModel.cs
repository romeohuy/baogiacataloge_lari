using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Estimate;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Models.Catalog;

namespace Nop.Web.Models.Estimate
{
    public class EstimatePriceModel
    {
        public EstimatePriceModel()
        {
            ProductByTypeEstimateSteps = new List<ProductByTypeEstimateStepModel>();
            EstimateInfoModel = new EstimateInfoModel();
        }
        public EstimateInfoModel EstimateInfoModel { get; set; }

        public List<ProductByTypeEstimateStepModel> ProductByTypeEstimateSteps { get; set; }
    }

    public class ProductByTypeEstimateStepModel
    {
        public ProductByTypeEstimateStepModel()
        {
            ProductItems = new List<SelectListItem>();
            ProductOverviewModels = new List<ProductOverviewModel>();
        }
        [NopResourceDisplayName("Web.ProductByTypeEstimateStep.Fields.TypeEstimateStep")]
        public int TypeEstimateStepId { get; set; }
        public TypeEstimateStep TypeEstimateStep { get; set; }

        [NopResourceDisplayName("Web.ProductByTypeEstimateStep.Fields.Product")]
        public int ProductId { get; set; }
        public List<SelectListItem> ProductItems { get; set; }
        public List<ProductOverviewModel> ProductOverviewModels { get; set; }
    }
}
