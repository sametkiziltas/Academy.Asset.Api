using System.Text.RegularExpressions;
using Academy.Asset.Api.Dtos;
using FluentValidation;

namespace Academy.Asset.Api.Validators;

public class AssetValidator : AbstractValidator<AssetDto>
{
    public AssetValidator()
    {
        RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Brand).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Model).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SerialNo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Status).IsInEnum();
    }
}
