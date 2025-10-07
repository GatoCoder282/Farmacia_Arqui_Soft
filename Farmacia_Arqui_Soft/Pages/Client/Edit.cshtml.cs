using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Factory;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using MySql.Data.MySqlClient; // <- para capturar MySqlException

using ClientEntity = Farmacia_Arqui_Soft.Models.Client;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class EditModel : PageModel
    {
        private readonly IRepository<ClientEntity> _repo;
        private readonly IValidator<ClientEntity> _validator;

        [BindProperty]
        public ClientEntity Input { get; set; }

        public EditModel(IValidator<ClientEntity> validator)
        {
            _validator = validator;

            var factory = new ClientRepositoryFactory();
            _repo = factory.CreateRepository<ClientEntity>();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var found = await _repo.GetById(id);
            if (found is null) return NotFound();
            Input = found;
            return Page();
        }

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
                await _repo.Update(Input);
            }
            catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
            {
                ModelState.AddModelError("Input.email", "Ese email ya está vinculado a otro cliente. Por favor, usa uno distinto.");
                return Page();
            }

            TempData["Success"] = "Cliente actualizado correctamente.";
            return RedirectToPage("/Client/IndexClient");
        }
    }
}
