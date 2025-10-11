using System.Text.RegularExpressions;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Validations.Interfaces;

namespace Farmacia_Arqui_Soft.Validations.Providers
{
    public class ProviderValidator : IValidator<Provider>
    {
        // Reglas base: requeridos, formatos y rangos ligeros
        // (La unicidad de nit/email la garantizamos en BD con UNIQUE; si quieres, luego agregamos verificación a BD aquí.)

        public ValidationResult Validate(Provider p)
        {
            var result = new ValidationResult();

            // --- FIRST NAME ---
            if (string.IsNullOrWhiteSpace(p.firstName))
                result.AddError("firstName", "El nombre es obligatorio.");
            else if (p.firstName.Length < 2)
                result.AddError("firstName", "El nombre debe tener al menos 2 caracteres.");

            // --- LAST NAME ---
            if (string.IsNullOrWhiteSpace(p.lastName))
                result.AddError("lastName", "El apellido es obligatorio.");
            else if (p.lastName.Length < 2)
                result.AddError("lastName", "El apellido debe tener al menos 2 caracteres.");

            // --- NIT (opcional, solo dígitos) ---
            if (!string.IsNullOrWhiteSpace(p.nit) && !Regex.IsMatch(p.nit, @"^\d+$"))
                result.AddError("nit", "El NIT solo debe contener números.");

            // --- EMAIL (opcional, formato básico) ---
            if (!string.IsNullOrWhiteSpace(p.email))
            {
                // Regex simple y suficiente para vista/servidor; la verificación fuerte la hará el UNIQUE de la BD
                var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(p.email, emailRegex))
                    result.AddError("email", "Correo electrónico no válido.");
            }

            // --- PHONE (opcional, dígitos, +, espacios y guiones) ---
            if (!string.IsNullOrWhiteSpace(p.phone) && !Regex.IsMatch(p.phone, @"^[\d\+\-\s]+$"))
                result.AddError("phone", "El teléfono solo puede contener dígitos, +, - y espacios.");

            // --- ADDRESS (opcional) ---
            if (!string.IsNullOrWhiteSpace(p.address) && p.address.Length > 500)
                result.AddError("address", "La dirección no debe exceder 500 caracteres.");

            // --- STATUS (byte) ---
            // Si manejas 0=inactivo, 1=activo:
            if (p.status != 0 && p.status != 1)
                result.AddError("status", "Status inválido. Use 0 (inactivo) o 1 (activo).");

            return result;
        }
    }
}
