using Farmacio_API.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Farmacio_API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errorResponse = GenerateValidationErrorResponse(context);
                context.Result = new BadRequestObjectResult(errorResponse);

                return;
            }

            await next();
        }

        private ValidationErrorResponse GenerateValidationErrorResponse(ActionExecutingContext context)
        {
            var errorsInModelState = context.ModelState
                                            .Where(property => property.Value.Errors.Count > 0)
                                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(error => error.ErrorMessage)).ToArray();

            var validationErrorResponse = new ValidationErrorResponse();

            foreach (var error in errorsInModelState)
            {
                foreach (var subErrorMessage in error.Value)
                {
                    var validationErrorModel = new ValidationErrorModel
                    {
                        Property = error.Key,
                        Message = subErrorMessage
                    };

                    validationErrorResponse.Errors.Add(validationErrorModel);
                }
            }

            return validationErrorResponse;
        }
    }
}