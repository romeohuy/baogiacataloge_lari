using Nop.Core.Domain.Estimate;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Estimate
{
    public class ProductEstimateModel : BaseNopEntityModel
    {
        public int ProductId { get; set; }
        public int ShoppingCartItemId { get; set; }
        public int EstimateId { get; set; }
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
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.HandPrint")]
        public int? HandPrint { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.ProductNote")]
        public string ProductNote { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.TypeProductPrint")]
        public int TypeProductPrintId { get; set; }
        public TypeProductPrint TypeProductPrint
        {
            get => (TypeProductPrint)TypeProductPrintId;
            set => TypeProductPrintId = (int)value;
        }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.TypeEstimateStep")]
        public int TypeEstimateStepId { get; set; }
        public TypeEstimateStep TypeEstimateStep
        {
            get => (TypeEstimateStep)TypeEstimateStepId;
            set => TypeEstimateStepId = (int)value;
        }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Quantity")]
        public int Quantity { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.CustomerEnteredPrice")]
        public decimal CustomerEnteredPrice { get; set; }
    }
}
