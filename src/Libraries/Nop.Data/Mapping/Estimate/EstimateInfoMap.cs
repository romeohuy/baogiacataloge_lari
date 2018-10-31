using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Estimate;

namespace Nop.Data.Mapping.Estimate
{
    public partial class EstimateInfoMap : NopEntityTypeConfiguration<EstimateInfo>
    {
        public override void Configure(EntityTypeBuilder<EstimateInfo> builder)
        {
            builder.ToTable(nameof(EstimateInfo));
            builder.HasKey(est => est.Id);

            builder.Ignore(est => est.TypeEstimate);

            base.Configure(builder);
        }
    }
}
