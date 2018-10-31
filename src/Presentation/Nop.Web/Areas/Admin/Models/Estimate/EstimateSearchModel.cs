using System;

namespace Nop.Web.Areas.Admin.Models.Estimate
{
    public class EstimateSearchModel
    {
        public string Termp { get; set; }
        public bool? IsFinish { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
