using Microsoft.AspNetCore.Mvc;

namespace BankProject;

public class ValidationProblemDetailsResult : IActionResult
{
    public async Task ExecuteResultAsync(ActionContext context)
    {
        var dic = context.ModelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        );

        CustomValidationProblemDetails problemDetails = new CustomValidationProblemDetails(dic){
            Status = StatusCodes.Status400BadRequest,
            Title = null
        };

        var objectResult = new ObjectResult(problemDetails) {
            StatusCode = problemDetails.Status
        };
        await objectResult.ExecuteResultAsync(context);
    }
}
