using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Farmacia_Arqui_Soft.Factory;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Models;

// Usings para el sistema de validaciones (nueva carpeta /Validations)
using Farmacia_Arqui_Soft.Validations.Interfaces;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<User> _userRepository;
        private readonly IValidator<User> _userValidator;

        public CreateModel(RepositoryFactory factory, IValidator<User> userValidator)
        {
            // Mantenemos Factory Method para crear el repositorio concreto
            _userRepository = factory.CreateRepository<User>();
            // Inyectamos un validador dedicado para User
            _userValidator = userValidator;
        }

        [BindProperty]
        public User User { get; set; } = new User();

        public void OnGet()
        {
            // Nada que hacer en GET
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            // Validación de binding (requeridos, tipos, etc.)
            if (!ModelState.IsValid)
                return Page();

            // Validación de reglas de negocio (username, password, phone, ci)
            var result = _userValidator.Validate(User);
            if (!result.IsValid)
            {
                // Mapeamos errores a ModelState para que salgan en la vista junto a cada campo
                foreach (var error in result.Errors)
                {
                    // El span de la vista espera claves tipo "User.campo"
                    var key = error.Key.StartsWith("User.") ? error.Key : $"User.{error.Key}";
                    ModelState.AddModelError(key, error.Value);
                }

                return Page();
            }

            // Persistir si todo es válido
            await _userRepository.Create(User);
            TempData["Success"] = "Usuario creado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
