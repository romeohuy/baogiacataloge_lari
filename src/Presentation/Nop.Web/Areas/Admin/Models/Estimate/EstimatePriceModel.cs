using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Estimate;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Web.Areas.Admin.Models.Estimate
{
    public class EstimatePriceModel
    {
        public EstimatePriceModel()
        {
            ProductByTypeEstimateSteps = new List<ProductByTypeEstimateStepModel>();
            EstimateInfoModel = new EstimateInfoModel();
            EstimateInfoNewModels = new List<EstimateInfoModel>();
        }

        public List<EstimateInfoModel> EstimateInfoNewModels { get; set; }
        public EstimateInfoModel EstimateInfoModel { get; set; }

        public List<ProductByTypeEstimateStepModel> ProductByTypeEstimateSteps { get; set; }
    }

    public class ProductByTypeEstimateStepModel
    {
        public ProductByTypeEstimateStepModel()
        {
            ProductItems = new List<SelectListItem>();
            ProductModels = new List<ProductModel>();
        }
        [NopResourceDisplayName("Web.ProductByTypeEstimateStep.Fields.TypeEstimateStep")]
        public int TypeEstimateStepId { get; set; }
        public TypeEstimateStep TypeEstimateStep { get; set; }

        [NopResourceDisplayName("Web.ProductByTypeEstimateStep.Fields.Product")]
        public int ProductId { get; set; }
        public List<SelectListItem> ProductItems { get; set; }
        public List<ProductModel> ProductModels { get; set; }
    }
}
