using Nop.Core.Domain.Estimate;
using Nop.Core.Domain.Orders;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;

namespace Nop.Web.Areas.Admin.Models.ShoppingCart
{
    /// <summary>
    /// Represents a shopping cart item model
    /// </summary>
    public partial class ShoppingCartItemModel : BaseNopEntityModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.CurrentCarts.Store")]
        public string Store { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.Product")]
        public int ProductId { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.Product")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.ShortDescription")]
        public string ShortDescription { get; set; }



        public string AttributeInfo { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.UnitName")]
        public string UnitName { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.HandPrint")]
        public int? HandPrint { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.UnitPrice")]
        public string UnitPrice { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.Quantity")]
        public int Quantity { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.Total")]
        public string Total { get; set; }
        public decimal TotalDec { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.Weight")]
        public decimal Weight { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.Length")]
        public decimal Length { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.Width")]
        public decimal Width { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.Height")]
        public decimal Height { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.TypeEstimateStep")]
        public int TypeEstimateStepId { get; set; }
        public TypeEstimateStep TypeEstimateStep
        {
            get => (TypeEstimateStep)TypeEstimateStepId;
            set => TypeEstimateStepId = (int)value;
        }

        public int ShoppingCartTypeId { get; set; }

        public ShoppingCartType ShoppingCartType
        {
            get => (ShoppingCartType)ShoppingCartTypeId;
            set => ShoppingCartTypeId = (int)value;
        }
        #endregion
    }
}