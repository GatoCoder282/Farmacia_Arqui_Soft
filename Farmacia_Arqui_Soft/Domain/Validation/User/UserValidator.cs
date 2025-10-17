using System.Text.RegularExpressions;
using System.Globalization;
using FluentResults;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Validations.Core;

namespace Farmacia_Arqui_Soft.Validations.Users
{
    public class UserValidator : IValidator<User>
    {
        private static readonly Regex AlphaSpaceRegex = new(@"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s]+$", RegexOptions.Compiled);
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private static readonly Regex DigitsRegex = new(@"^\d+$", RegexOptions.Compiled);

        public Result Validate(User user)
        {
            var result = Result.Ok();

            result = ValidateFirstName(user, result);
            result = ValidateSecondName(user, result);
            result = ValidateLastName(user, result);
            result = ValidateEmail(user, result);
            result = ValidateCi(user, result);
            result = ValidatePhone(user, result);

            return result;
        }

        private Result ValidateFirstName(User u, Result result)
        {
            if (string.IsNullOrWhiteSpace(u.first_name))
                return result.WithFieldError("first_name", "El nombre es obligatorio.");

            if (u.first_name.Length < 2 || u.first_name.Length > 50)
                result = result.WithFieldError("first_name", "El nombre debe tener entre 2 y 50 caracteres.");

            if (!AlphaSpaceRegex.IsMatch(u.first_name))
                result = result.WithFieldError("first_name", "El nombre solo debe contener letras y espacios.");

            return result;
        }

        private Result ValidateSecondName(User u, Result result)
        {
            if (string.IsNullOrWhiteSpace(u.second_name))
                return result;

            if (u.second_name.Length < 2 || u.second_name.Length > 50)
                result = result.WithFieldError("second_name", "El segundo nombre debe tener entre 2 y 50 caracteres.");

            if (!AlphaSpaceRegex.IsMatch(u.second_name))
                result = result.WithFieldError("second_name", "El segundo nombre solo debe contener letras y espacios.");

            return result;
        }

        private Result ValidateLastName(User u, Result result)
        {
            if (string.IsNullOrWhiteSpace(u.last_name))
                return result.WithFieldError("last_name", "El apellido es obligatorio.");

            if (u.last_name.Length < 2 || u.last_name.Length > 50)
                result = result.WithFieldError("last_name", "El apellido debe tener entre 2 y 50 caracteres.");

            if (!AlphaSpaceRegex.IsMatch(u.last_name))
                result = result.WithFieldError("last_name", "El apellido solo debe contener letras y espacios.");

            return result;
        }

        private Result ValidateEmail(User u, Result result)
        {
            if (string.IsNullOrWhiteSpace(u.mail))
                return result.WithFieldError("mail", "El correo es obligatorio.");

            if (u.mail.Length > 100)
                result = result.WithFieldError("mail", "El correo no debe exceder 100 caracteres.");

            if (!EmailRegex.IsMatch(u.mail))
                result = result.WithFieldError("mail", "El correo no tiene un formato válido.");

            return result;
        }

        private Result ValidateCi(User u, Result result)
        {
            if (string.IsNullOrWhiteSpace(u.ci))
                return result.WithFieldError("ci", "El CI es obligatorio.");

            if (!DigitsRegex.IsMatch(u.ci))
                result = result.WithFieldError("ci", "El CI solo debe contener números.");

            if (u.ci.Length < 5 || u.ci.Length > 12)
                result = result.WithFieldError("ci", "El CI debe tener entre 5 y 12 dígitos.");

            return result;
        }

        private Result ValidatePhone(User u, Result result)
        {
            if (u.phone <= 0)
                return result.WithFieldError("phone", "El teléfono debe ser un número positivo.");

            var digits = u.phone.ToString(CultureInfo.InvariantCulture).Length;
            if (digits < 6 || digits > 10)
                result = result.WithFieldError("phone", "El teléfono debe tener entre 6 y 10 dígitos.");

            return result;
        }
    }
}
