using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Estimate;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Areas.Admin.Validators.Estimate;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Models.Estimate
{
    [Validator(typeof(EstimateInfoValidator))]
    public class EstimateInfoModel : BaseNopEntityModel
    {
        
        [NopResourceDisplayName("Web.EstimateInfo.Fields.CustomerFirstName")]
        public string CustomerFirstName { get; set; }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.CustomerLastName")]
        public string CustomerLastName { get; set; }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.CustomerPhone")]
        public string CustomerPhone { get; set; }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.NumberEstimate")]
        public string NumberEstimate { get;set; }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.Title")]
        public string Title { get; set; }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.UnitName")]
        public string UnitName { get;set; }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.TotalNumber")]
        public int TotalNumber { get;set; }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.ProductTypeName")]
        public string ProductTypeName { get; set; }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.ProductName")]
        public string ProductName { get; set; }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.ExtendNote")]
        public string ExtendNote { get; set; }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.CreatedDate")]
        [UIHint("DateTime")]
        public DateTime CreatedDate { get; set; }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.TypeEstimateId")]
        public int TypeEstimateId { get; set; }

        public TypeEstimate TypeEstimate
        {
            get => (TypeEstimate)TypeEstimateId;
            set => TypeEstimateId = (int)value;
        }

        public EstimateInfoModel()
        {
            Customers = new List<SelectListItem>();
        }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.CustomerFullName")]
        public int CustomerId { get; set; }
        [NopResourceDisplayName("Web.EstimateInfo.Fields.Customers")]
        public List<SelectListItem> Customers { get; set; }

    }
}
