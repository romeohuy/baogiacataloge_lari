using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Domain.Estimate;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Models.Estimate
{
    public class EstimateProductModel
    {
        public int ProductId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.TypeProductPrint")]
        public int TypeProductPrintId { get; set; }
        public TypeProductPrint TypeProductPrint { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Price")]
        public decimal Price { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Quantity")]
        public int Quantity { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Weight")]
        public decimal Weight { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Length")]
        public decimal Length { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Width")]
        public decimal Width { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Height")]
        public decimal Height { get; set; }
    }
}
