using FluentResults;

namespace Farmacia_Arqui_Soft.Validations.Core
{
    public static class ResultExtensions
    {
      
        public static Result WithFieldError(this Result result, string field, string message)
        {
            var error = new Error(message).WithMetadata("field", field);
            return result.WithError(error);
        }
    }
}
