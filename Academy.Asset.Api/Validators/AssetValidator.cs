using System.Text.RegularExpressions;
using FluentValidation;

namespace Academy.Asset.Api.Validators;

public class AssetValidator : AbstractValidator<Domain.Asset>
{
    public AssetValidator()
    {
        RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Brand).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Model).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SerialNo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Status).IsInEnum();
        
        When(x => x.Tag is not null, () =>
        {
            RuleFor(x => x.Tag).SetValidator(new TagValidator());
        });
    }
}
