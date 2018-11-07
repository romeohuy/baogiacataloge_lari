using Nop.Core.Domain.Estimate;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Estimate
{
    public class EstimateSummaryModel
    {
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.TongTienGiaCong")]
        public string TongTienGiaCong { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.TienBia")]
        public string TienBia { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.TienRuot")]
        public string TienRuot { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.TongTienHang")]
        public string TongTienHang { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.LoiNhuan")]
        public string LoiNhuan { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.TienLoiNhuan")]
        public string TienLoiNhuan { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.TongDonHang")]
        public string TongDonHang { get; set; }
        [NopResourceDisplayName("Admin.EstimateInfo.Fields.DonGiaSanPham")]
        public string DonGiaSanPham { get; set; }
        public int TypeEstimateId { get; set; }

        public TypeEstimate TypeEstimate
        {
            get => (TypeEstimate)TypeEstimateId;
            set => TypeEstimateId = (int)value;
        }

    }
}
