using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Customers;

namespace Nop.Core.Domain.Estimate
{
   public partial class EstimateInfo : BaseEntity
    {
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerPhone { get; set; }
        public string NumberEstimate { get;set; }
        public string Title { get; set; }
        public string UnitName { get;set; }
        public int TotalNumber { get;set; }
        public string ProductTypeName { get; set; }
        public string ProductName { get; set; }
        public string ExtendNote { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TypeEstimateId { get; set; }
        public bool IsFinish { get; set; }
        public TypeEstimate TypeEstimate
        {
            get => (TypeEstimate)TypeEstimateId;
            set => TypeEstimateId = (int)value;
        }
        
    }
}
