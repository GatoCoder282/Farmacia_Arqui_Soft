using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Models;


using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Repository;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<User> _userRepository;
        private readonly IValidator<User> _userValidator;

        public CreateModel(RepositoryFactory factory, IValidator<User> userValidator)
        {
            
            _userRepository = factory.CreateRepository<User>();
            _userValidator = userValidator;
        }

        [BindProperty]
        public User User { get; set; } = new User();

        public void OnGet()
        {
           
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

           
            await _userRepository.Create(User);
            TempData["Success"] = "Usuario creado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
