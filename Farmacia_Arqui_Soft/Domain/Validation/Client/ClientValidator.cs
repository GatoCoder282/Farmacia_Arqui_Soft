using System.Text.RegularExpressions;
using FluentResults;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Validations.Core;

namespace Farmacia_Arqui_Soft.Validations.Clients
{
    public class ClientValidator : IValidator<Client>
    {
        public Result Validate(Client client)
        {
            var result = Result.Ok();

            result = ValidateFirstName(client, result);
            result = ValidateLastName(client, result);
            result = ValidateNit(client, result);
            result = ValidateEmail(client, result);

            return result;
        }

        private Result ValidateFirstName(Client client, Result result)
        {
            if (string.IsNullOrWhiteSpace(client.first_name))
                return result.WithFieldError("first_name", "El nombre es obligatorio.");

            if (client.first_name.Trim().Length < 2)
                result = result.WithFieldError("first_name", "El nombre debe tener al menos 2 caracteres.");

            if (!Regex.IsMatch(client.first_name, @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s]+$"))
                result = result.WithFieldError("first_name", "El nombre solo debe contener letras y espacios.");

            return result;
        }

        private Result ValidateLastName(Client client, Result result)
        {
            if (string.IsNullOrWhiteSpace(client.last_name))
                return result.WithFieldError("last_name", "El apellido es obligatorio.");

            if (client.last_name.Trim().Length < 2)
                result = result.WithFieldError("last_name", "El apellido debe tener al menos 2 caracteres.");

            if (!Regex.IsMatch(client.last_name, @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s]+$"))
                result = result.WithFieldError("last_name", "El apellido solo debe contener letras y espacios.");

            return result;
        }

        private Result ValidateNit(Client client, Result result)
        {
            if (string.IsNullOrWhiteSpace(client.nit))
                return result;

            if (!Regex.IsMatch(client.nit, @"^\d+$"))
                result = result.WithFieldError("nit", "El NIT solo debe contener números.");

            if (client.nit.Length < 6 || client.nit.Length > 15)
                result = result.WithFieldError("nit", "El NIT debe tener entre 6 y 15 dígitos.");

            return result;
        }

        private Result ValidateEmail(Client client, Result result)
        {
            if (string.IsNullOrWhiteSpace(client.email))
                return result;

            if (!Regex.IsMatch(client.email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                result = result.WithFieldError("email", "El email no tiene un formato válido.");

            return result;
        }
    }
}
