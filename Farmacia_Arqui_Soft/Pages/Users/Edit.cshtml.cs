using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

// Validaciones
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IRepository<User> _userRepository;
        private readonly IValidator<User> _userValidator;

        public EditModel(RepositoryFactory factory, IValidator<User> userValidator)
        {
            _userRepository = factory.CreateRepository<User>();
            _userValidator = userValidator;
        }

        [BindProperty]
        public User User { get; set; } = new User();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var tempUser = new User { id = id };
            var existing = await _userRepository.GetById(tempUser);
            if (existing == null)
                return RedirectToPage("Index"); // o NotFound()

            User = existing;
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Validación de negocio
            var result = _userValidator.Validate(User);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    var key = error.Key.StartsWith("User.") ? error.Key : $"User.{error.Key}";
                    ModelState.AddModelError(key, error.Value);
                }
                return Page();
            }

            await _userRepository.Update(User);
            TempData["Success"] = "Usuario actualizado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
