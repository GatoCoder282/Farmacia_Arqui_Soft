using System.Text.RegularExpressions;
using FluentResults;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Validations.Core;

namespace Farmacia_Arqui_Soft.Validations.Providers
{
    public class ProviderValidator : IValidator<Provider>
    {
        public Result Validate(Provider provider)
        {
            var result = Result.Ok();

            result = ValidateFirstName(provider, result);
            result = ValidateLastName(provider, result);
            result = ValidateNit(provider, result);
            result = ValidateEmail(provider, result);
            result = ValidatePhone(provider, result);
            result = ValidateAddress(provider, result);
            result = ValidateStatus(provider, result);

            return result;
        }

        private Result ValidateFirstName(Provider p, Result result)
        {
            if (string.IsNullOrWhiteSpace(p.firstName))
                return result.WithFieldError("firstName", "El nombre es obligatorio.");

            if (p.firstName.Trim().Length < 2)
                result = result.WithFieldError("firstName", "El nombre debe tener al menos 2 caracteres.");

            if (!Regex.IsMatch(p.firstName, @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s]+$"))
                result = result.WithFieldError("firstName", "El nombre solo debe contener letras y espacios.");

            return result;
        }

        private Result ValidateLastName(Provider p, Result result)
        {
            if (string.IsNullOrWhiteSpace(p.lastName))
                return result.WithFieldError("lastName", "El apellido es obligatorio.");

            if (p.lastName.Trim().Length < 2)
                result = result.WithFieldError("lastName", "El apellido debe tener al menos 2 caracteres.");

            if (!Regex.IsMatch(p.lastName, @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s]+$"))
                result = result.WithFieldError("lastName", "El apellido solo debe contener letras y espacios.");

            return result;
        }

        private Result ValidateNit(Provider p, Result result)
        {
            if (!string.IsNullOrWhiteSpace(p.nit) && !Regex.IsMatch(p.nit, @"^\d+$"))
                result = result.WithFieldError("nit", "El NIT solo debe contener números.");

            return result;
        }

        private Result ValidateEmail(Provider p, Result result)
        {
            if (!string.IsNullOrWhiteSpace(p.email) && 
                !Regex.IsMatch(p.email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                result = result.WithFieldError("email", "Correo electrónico no válido.");

            return result;
        }

        private Result ValidatePhone(Provider p, Result result)
        {
            if (string.IsNullOrWhiteSpace(p.phone))
                return result;

            if (!Regex.IsMatch(p.phone, @"^[\d\+\-\s]+$"))
                result = result.WithFieldError("phone", "El teléfono solo puede contener dígitos, +, - y espacios.");

            var cleaned = p.phone.Replace(" ", "").Replace("-", "").Replace("+", "");
            if (cleaned.Length < 6)
                result = result.WithFieldError("phone", "El teléfono es demasiado corto.");

            return result;
        }

        private Result ValidateAddress(Provider p, Result result)
        {
            if (!string.IsNullOrWhiteSpace(p.address) && p.address.Length > 500)
                result = result.WithFieldError("address", "La dirección no debe exceder 500 caracteres.");

            return result;
        }

        private Result ValidateStatus(Provider p, Result result)
        {
            if (p.status != 0 && p.status != 1)
                result = result.WithFieldError("status", "Status inválido. Use 0 (inactivo) o 1 (activo).");

            return result;
        }
    }
}
