using System.Text.RegularExpressions;
using Academy.Asset.Api.Domain;
using FluentValidation;

namespace Academy.Asset.Api.Validators;

public class TagValidator : AbstractValidator<Tag>
{
    public TagValidator()
    {
        RuleFor(x => x.MacAddress).ValidMacAddress().NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}


public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> ValidMacAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(x => x is not null && Regex.IsMatch(x, "^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$"))
            .WithMessage("Invalid MAC address");
    }
}