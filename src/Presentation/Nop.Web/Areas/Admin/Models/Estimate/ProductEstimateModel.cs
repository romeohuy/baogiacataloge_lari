using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Estimate
{
    public class ProductEstimateModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Name")]
        public string Name { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Sku")]
        public string Sku { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Weight")]
        public decimal Weight { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Length")]
        public decimal Length { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Width")]
        public decimal Width { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Height")]
        public decimal Height { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.UnitName")]
        public string UnitName { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.ProductNote")]
        public string ProductNote { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.TypeProductPrint")]
        public int TypeProductPrintId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.TypeProductPrint")]
        public string TypeProductPrintName { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.TypeEstimateStep")]
        public int TypeEstimateStepId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.TypeEstimateStep")]
        public string TypeEstimateStepName { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.TotalNumber")]
        public int TotalNumber { get; set; }
    }
}
