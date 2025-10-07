using System.Text.RegularExpressions;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Validations.Interfaces;

namespace Farmacia_Arqui_Soft.Validations.Clients
{
    public class ClientValidator : IValidator<Client>
    {
        public ValidationResult Validate(Client client)
        {
            var result = new ValidationResult();

            // FIRST NAME: requerido, solo letras y espacios, mínimo 2
            if (string.IsNullOrWhiteSpace(client.first_name))
            {
                result.AddError("first_name", "El nombre es obligatorio.");
            }
            else
            {
                if (client.first_name.Trim().Length < 2)
                    result.AddError("first_name", "El nombre debe tener al menos 2 caracteres.");

                if (!Regex.IsMatch(client.first_name, @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s]+$"))
                    result.AddError("first_name", "El nombre solo debe contener letras y espacios.");
            }

            // LAST NAME: requerido, solo letras y espacios, mínimo 2
            if (string.IsNullOrWhiteSpace(client.last_name))
            {
                result.AddError("last_name", "El apellido es obligatorio.");
            }
            else
            {
                if (client.last_name.Trim().Length < 2)
                    result.AddError("last_name", "El apellido debe tener al menos 2 caracteres.");

                if (!Regex.IsMatch(client.last_name, @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s]+$"))
                    result.AddError("last_name", "El apellido solo debe contener letras y espacios.");
            }

            // NIT: opcional, pero si viene debe ser solo dígitos (largo 6-15 común en BO)
            if (!string.IsNullOrWhiteSpace(client.nit))
            {
                if (!Regex.IsMatch(client.nit, @"^\d+$"))
                    result.AddError("nit", "El NIT solo debe contener números.");
                else if (client.nit.Length < 6 || client.nit.Length > 15)
                    result.AddError("nit", "El NIT debe tener entre 6 y 15 dígitos.");
            }

            // EMAIL: opcional, pero si viene debe tener formato válido
            if (!string.IsNullOrWhiteSpace(client.email))
            {
                // Regex simple y suficiente para validación básica
                if (!Regex.IsMatch(client.email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    result.AddError("email", "El email no tiene un formato válido.");
            }

            return result;
        }
    }
}
