using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Estimate;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Estimate
{
    public class EstimateInfoValidator : BaseNopValidator<EstimateInfoModel>
    {
        public EstimateInfoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ProductTypeName).NotEmpty().WithMessage(localizationService.GetResource("EstimateInfo.Fields.ProductTypeName.Required"));
            RuleFor(x => x.CustomerFirstName).NotEmpty().WithMessage(localizationService.GetResource("EstimateInfo.Fields.CustomerFirstName.Required"));
            RuleFor(x => x.CustomerLastName).NotEmpty().WithMessage(localizationService.GetResource("EstimateInfo.Fields.CustomerLastName.Required"));
            RuleFor(x => x.CustomerPhone).NotEmpty().WithMessage(localizationService.GetResource("EstimateInfo.Fields.CustomerPhone.Required"));
            RuleFor(x => x.Title).NotEmpty().WithMessage(localizationService.GetResource("EstimateInfo.Fields.Title.Required"));
            RuleFor(x => x.TotalNumber).NotEmpty().WithMessage(localizationService.GetResource("EstimateInfo.Fields.TotalNumber.Required"));
            RuleFor(x => x.UnitNameNote).NotEmpty().WithMessage(localizationService.GetResource("EstimateInfo.Fields.UnitName.Required"));
            RuleFor(x => x.ExtendNote).NotEmpty().WithMessage(localizationService.GetResource("EstimateInfo.Fields.ExtendNote.Required"));
            RuleFor(x => x.CreatedDate).NotEmpty().WithMessage(localizationService.GetResource("EstimateInfo.Fields.CreatedDate.Required"));
        }
    }
}
