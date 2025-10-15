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
