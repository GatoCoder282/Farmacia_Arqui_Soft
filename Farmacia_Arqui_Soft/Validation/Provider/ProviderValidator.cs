using System.Text.RegularExpressions;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Validations.Interfaces;

namespace Farmacia_Arqui_Soft.Validations.Providers
{
    public class ProviderValidator : IValidator<Provider>
    {
        public ValidationResult Validate(Provider p)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(p.firstName))
                result.AddError("firstName", "El nombre es obligatorio.");
            else if (p.firstName.Length < 2)
                result.AddError("firstName", "El nombre debe tener al menos 2 caracteres.");

            if (string.IsNullOrWhiteSpace(p.lastName))
                result.AddError("lastName", "El apellido es obligatorio.");
            else if (p.lastName.Length < 2)
                result.AddError("lastName", "El apellido debe tener al menos 2 caracteres.");

            if (!string.IsNullOrWhiteSpace(p.nit) && !Regex.IsMatch(p.nit, @"^\d+$"))
                result.AddError("nit", "El NIT solo debe contener números.");

            if (!string.IsNullOrWhiteSpace(p.email))
            {
                var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(p.email, emailRegex))
                    result.AddError("email", "Correo electrónico no válido.");
            }

            if (!string.IsNullOrWhiteSpace(p.phone) && !Regex.IsMatch(p.phone, @"^[\d\+\-\s]+$"))
                result.AddError("phone", "El teléfono solo puede contener dígitos, +, - y espacios.");

            if (!string.IsNullOrWhiteSpace(p.address) && p.address.Length > 500)
                result.AddError("address", "La dirección no debe exceder 500 caracteres.");

            if (p.status != 0 && p.status != 1)
                result.AddError("status", "Status inválido. Use 0 (inactivo) o 1 (activo).");

            return result;
        }
    }
}
