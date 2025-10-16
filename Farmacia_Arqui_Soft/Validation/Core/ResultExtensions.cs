using FluentResults;

namespace Farmacia_Arqui_Soft.Validations.Core
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Agrega un error de validación asociado a un campo específico.
        /// El nombre del campo se guarda en Metadata["field"].
        /// </summary>
        public static Result WithFieldError(this Result result, string field, string message)
        {
            var error = new Error(message).WithMetadata("field", field);
            return result.WithError(error);
        }
    }
}
