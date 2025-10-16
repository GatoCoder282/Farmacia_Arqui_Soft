using System.Text.RegularExpressions;
using FluentResults;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Validations.Core;

namespace Farmacia_Arqui_Soft.Validations.Providers
{
    public class ProviderValidator : IValidator<Provider>
    {
        public Result Validate(Provider p)
        {
            var result = Result.Ok();

            // FIRST NAME: requerido, mín 2, solo letras y espacios
            if (string.IsNullOrWhiteSpace(p.firstName))
            {
                result = result.WithFieldError("firstName", "El nombre es obligatorio.");
            }
            else
            {
                if (p.firstName.Trim().Length < 2)
                    result = result.WithFieldError("firstName", "El nombre debe tener al menos 2 caracteres.");
                if (!Regex.IsMatch(p.firstName, @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s]+$"))
                    result = result.WithFieldError("firstName", "El nombre solo debe contener letras y espacios.");
            }

            // LAST NAME: requerido, mín 2, solo letras y espacios
            if (string.IsNullOrWhiteSpace(p.lastName))
            {
                result = result.WithFieldError("lastName", "El apellido es obligatorio.");
            }
            else
            {
                if (p.lastName.Trim().Length < 2)
                    result = result.WithFieldError("lastName", "El apellido debe tener al menos 2 caracteres.");
                if (!Regex.IsMatch(p.lastName, @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s]+$"))
                    result = result.WithFieldError("lastName", "El apellido solo debe contener letras y espacios.");
            }

            // NIT (opcional, solo dígitos)
            if (!string.IsNullOrWhiteSpace(p.nit))
            {
                if (!Regex.IsMatch(p.nit, @"^\d+$"))
                    result = result.WithFieldError("nit", "El NIT solo debe contener números.");
            }

            // EMAIL (opcional, formato básico)
            if (!string.IsNullOrWhiteSpace(p.email))
            {
                var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(p.email, emailRegex))
                    result = result.WithFieldError("email", "Correo electrónico no válido.");
            }

            // PHONE (opcional, dígitos, +, -, espacios)
            if (!string.IsNullOrWhiteSpace(p.phone))
            {
                if (!Regex.IsMatch(p.phone, @"^[\d\+\-\s]+$"))
                    result = result.WithFieldError("phone", "El teléfono solo puede contener dígitos, +, - y espacios.");
                // si quieres rangos típicos BO: 6–15
                if (p.phone.Replace(" ", "").Replace("-", "").Replace("+", "").Length < 6)
                    result = result.WithFieldError("phone", "El teléfono es demasiado corto.");
            }

            // ADDRESS (opcional)
            if (!string.IsNullOrWhiteSpace(p.address) && p.address.Length > 500)
                result = result.WithFieldError("address", "La dirección no debe exceder 500 caracteres.");

            // STATUS (byte): 0 o 1
            if (p.status != 0 && p.status != 1)
                result = result.WithFieldError("status", "Status inválido. Use 0 (inactivo) o 1 (activo).");

            return result;
        }
    }
}
