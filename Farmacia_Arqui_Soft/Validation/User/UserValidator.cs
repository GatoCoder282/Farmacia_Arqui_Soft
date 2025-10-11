using System.Text.RegularExpressions;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Validations.Interfaces;

namespace Farmacia_Arqui_Soft.Validations.Users
{
    public class UserValidator : IValidator<User>
    {
        public ValidationResult Validate(User user)
        {
            var result = new ValidationResult();

            // --- USERNAME ---
            if (string.IsNullOrWhiteSpace(user.username))
                result.AddError("username", "El nombre de usuario es obligatorio.");
            else if (user.username.Length < 4)
                result.AddError("username", "El nombre de usuario debe tener al menos 4 caracteres.");

            // --- PASSWORD ---
            if (string.IsNullOrWhiteSpace(user.password))
                result.AddError("password", "La contraseña es obligatoria.");
            else if (user.password.Length <= 3)
                result.AddError("password", "La contraseña debe tener más de 3 caracteres.");
            else if (!Regex.IsMatch(user.password, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$"))
                result.AddError("password", "La contraseña debe contener letras y números.");

            // --- PHONE ---
            if (user.phone.ToString().Any(c => !char.IsDigit(c)))
                result.AddError("phone", "El teléfono solo debe contener números.");

            // --- CI ---
            if (string.IsNullOrWhiteSpace(user.ci))
                result.AddError("ci", "El CI es obligatorio.");
            else if (!Regex.IsMatch(user.ci, @"^\d+$"))
                result.AddError("ci", "El CI solo debe contener números.");

            return result;
        }
    }
}
