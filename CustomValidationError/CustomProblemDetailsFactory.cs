using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BankProject;

public class CustomProblemDetailsFactory : ProblemDetailsFactory
{
    public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
    {
        ProblemDetails p = new ProblemDetails{
            Status = statusCode,
            Title = title,
            Instance = instance,
            Detail = detail,
            Type = type
        };
        return p;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, 
    int? statusCode = null, string? title = null, string? type = null, 
    string? detail = null, string? instance = null)
    {
        statusCode ??= 400;
        type ??= "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        instance ??= httpContext.Request.Path;

        CustomValidationProblemDetails custom = new CustomValidationProblemDetails(modelStateDictionary)
        {
            Instance = instance,
            Type = type,
            Status = statusCode
        };

        if(title != null) {
            custom.Title = title;
        }

        var traceId = Activity.Current.Id ?? httpContext.TraceIdentifier;

        if(traceId != null) {
            custom.Extensions["traceId"] = traceId;
        }

        return custom;
    }
}
