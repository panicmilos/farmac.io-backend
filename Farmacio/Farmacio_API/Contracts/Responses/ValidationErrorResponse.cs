using System.Collections.Generic;

namespace Farmacio_API.Contracts.Responses
{
    public class ValidationErrorResponse
    {
        public List<ValidationErrorModel> Errors { get; set; } = new List<ValidationErrorModel>();
    }
}