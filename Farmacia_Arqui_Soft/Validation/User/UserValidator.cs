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
        private static readonly Regex AlphaSpaceRegex =
            new Regex(@"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ ]+$", RegexOptions.Compiled);

        private static readonly Regex EmailRegex =
            new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        private static readonly Regex DigitsRegex =
            new Regex(@"^\d+$", RegexOptions.Compiled);

        public Result Validate(User user)
        {
            var result = Result.Ok();

            // NOMBRE (obligatorio, 2-50, letras/espacios)
            if (string.IsNullOrWhiteSpace(user.first_name))
                result = result.WithFieldError("first_name", "El nombre es obligatorio.");
            else
            {
                if (user.first_name.Length < 2 || user.first_name.Length > 50)
                    result = result.WithFieldError("first_name", "El nombre debe tener entre 2 y 50 caracteres.");
                if (!AlphaSpaceRegex.IsMatch(user.first_name))
                    result = result.WithFieldError("first_name", "El nombre solo debe contener letras y espacios.");
            }

            // SEGUNDO NOMBRE (opcional)
            if (!string.IsNullOrWhiteSpace(user.second_name))
            {
                if (user.second_name.Length < 2 || user.second_name.Length > 50)
                    result = result.WithFieldError("second_name", "El segundo nombre debe tener entre 2 y 50 caracteres.");
                if (!AlphaSpaceRegex.IsMatch(user.second_name))
                    result = result.WithFieldError("second_name", "El segundo nombre solo debe contener letras y espacios.");
            }

            // APELLIDO (obligatorio)
            if (string.IsNullOrWhiteSpace(user.last_name))
                result = result.WithFieldError("last_name", "El apellido es obligatorio.");
            else
            {
                if (user.last_name.Length < 2 || user.last_name.Length > 50)
                    result = result.WithFieldError("last_name", "El apellido debe tener entre 2 y 50 caracteres.");
                if (!AlphaSpaceRegex.IsMatch(user.last_name))
                    result = result.WithFieldError("last_name", "El apellido solo debe contener letras y espacios.");
            }

            // CORREO (obligatorio)
            if (string.IsNullOrWhiteSpace(user.mail))
                result = result.WithFieldError("mail", "El correo es obligatorio.");
            else
            {
                if (user.mail.Length > 100)
                    result = result.WithFieldError("mail", "El correo no debe exceder 100 caracteres.");
                if (!EmailRegex.IsMatch(user.mail))
                    result = result.WithFieldError("mail", "El correo no tiene un formato válido.");
            }

            // CI (obligatorio, solo dígitos, 5-12)
            if (string.IsNullOrWhiteSpace(user.ci))
                result = result.WithFieldError("ci", "El CI es obligatorio.");
            else
            {
                if (!DigitsRegex.IsMatch(user.ci))
                    result = result.WithFieldError("ci", "El CI solo debe contener números.");
                if (user.ci.Length < 5 || user.ci.Length > 12)
                    result = result.WithFieldError("ci", "El CI debe tener entre 5 y 12 dígitos.");
            }

            // TELÉFONO (int): obligatorio, positivo, 6–10 dígitos
            // *int* no guarda ceros a la izquierda, así que la longitud se evalúa sobre el valor entero.
            if (user.phone <= 0)
            {
                result = result.WithFieldError("phone", "El teléfono debe ser un número positivo.");
            }
            else
            {
                // Máximo real para int: 2,147,483,647 (10 dígitos)
                // Calculamos dígitos a partir del entero positivo
                var digits = user.phone.ToString(CultureInfo.InvariantCulture).Length;

                if (digits < 6 || digits > 10)
                    result = result.WithFieldError("phone", "El teléfono debe tener entre 6 y 10 dígitos.");
            }

            return result;
        }
    }
}
