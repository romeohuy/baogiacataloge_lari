using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Estimate;
using Nop.Web.Areas.Admin.Validators.Estimate;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Estimate
{
    [Validator(typeof(EstimateInfoValidator))]
    public class EstimateInfoModel : BaseNopEntityModel
    {

        [NopResourceDisplayName("Admin.EstimateInfo.Fields.CustomerFirstName")]
        public string CustomerFirstName { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.CustomerLastName")]
        public string CustomerLastName { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.CustomerPhone")]
        public string CustomerPhone { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.NumberEstimate")]
        public string NumberEstimate { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.Title")]
        public string Title { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.UnitName")]
        public string UnitName { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.TotalNumber")]
        public int TotalNumber { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.ProductTypeName")]
        public string ProductTypeName { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.ProductName")]
        public string ProductName { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.ExtendNote")]
        public string ExtendNote { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.CreatedDate")]
        [UIHint("Date")]
        public DateTime CreatedDate { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.TypeEstimateId")]
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
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.CustomerFullName")]
        public int CustomerId { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.Customers")]
        public List<SelectListItem> Customers { get; set; }

    }
}
