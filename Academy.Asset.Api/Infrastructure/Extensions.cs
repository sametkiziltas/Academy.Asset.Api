using System.Text.Json;
using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

namespace Academy.Asset.Api.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddAcademyProblemDetails(this IServiceCollection services, Action<ProblemDetailsOptions> configure = null)
    {
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        services.AddProblemDetails(
            problemDetail =>
            {
                problemDetail.Map<BusinessException>(ex => new BusinessExceptionProblemDetails(ex, serviceProvider));
                configure?.Invoke(problemDetail);
            });

        return services;
    }
}


public abstract class CustomException : System.Exception
{
    public CustomException(string key, string message)
        : base(message)
    {
        Key = key;
    }

    public string Key { get; set; }

    public string Detail => JsonSerializer.Serialize(new Dictionary<string, List<string>> { [Key] = new() { Message } });
}


public class BusinessException : CustomException
{
    public BusinessException(Type brokenRule, string message)
        : base(brokenRule.Name, message)
    {
        BrokenRule = brokenRule;
    }

    public BusinessException()
        : base(nameof(BusinessException), string.Empty)
    {
    }

    public BusinessException(string message)
        : base(nameof(BusinessException), message)
    {
    }

    public BusinessException(string key, string message)
        : base(key, message)
    {
    }

    public Type BrokenRule { get; }

    public static void ThrowIfFalse(string message, bool value)
    {
        if (!value)
        {
            throw new BusinessException(message, string.Empty);
        }
    }

    public static void ThrowIfTrue(string message, bool value)
    {
        if (value)
        {
            throw new BusinessException(nameof(message), message);
        }
    }

    public static void ThrowIfNull(string message, object obj)
    {
        if (obj is null)
        {
            throw new BusinessException(message, string.Empty);
        }
    }
}

public class BusinessExceptionProblemDetails : ProblemDetails
{
    public BusinessExceptionProblemDetails(BusinessException exception, IServiceProvider serviceProvider)
    {
        Title = "Business Error";
        Status = StatusCodes.Status400BadRequest;
        Type = "https://bordatech.com/business-exception";
    }
}

