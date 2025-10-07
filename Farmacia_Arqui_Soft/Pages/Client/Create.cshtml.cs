using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Factory;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using MySql.Data.MySqlClient; // <- para capturar MySqlException

// Alias para el modelo Client
using ClientEntity = Farmacia_Arqui_Soft.Models.Client;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<ClientEntity> _repo;
        private readonly IValidator<ClientEntity> _validator;

        [BindProperty]
        public ClientEntity Input { get; set; } = new ClientEntity { };

        // Mantiene Factory para el repositorio + inyección del validador
        public CreateModel(IValidator<ClientEntity> validator)
        {
            _validator = validator;

            var factory = new ClientRepositoryFactory();
            _repo = factory.CreateRepository<ClientEntity>();
        }

        public void OnGet() { }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Validación de negocio
            var result = _validator.Validate(Input);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    var key = error.Key.StartsWith("Input.") ? error.Key : $"Input.{error.Key}";
                    ModelState.AddModelError(key, error.Value);
                }
                return Page();
            }

            try
            {
                await _repo.Create(Input);
            }
            catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
            {
                // Mensaje amigable en el campo email
                ModelState.AddModelError("Input.email", "Ese email ya está vinculado a otro cliente. Por favor, usa uno distinto.");
                return Page();
            }

            TempData["Success"] = "Cliente creado correctamente.";
            return RedirectToPage("/Client/IndexClient");
        }
    }
}
