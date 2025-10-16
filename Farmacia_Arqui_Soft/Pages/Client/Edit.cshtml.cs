using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using MySql.Data.MySqlClient;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

using ClientEntity = Farmacia_Arqui_Soft.Domain.Models.Client;
using Farmacia_Arqui_Soft.Domain.Ports;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class EditModel : PageModel
    {
        private readonly IRepository<ClientEntity> _ClientRepository;
        private readonly IValidator<ClientEntity> _validator;

        [BindProperty]
        public ClientEntity Input { get; set; }

        public EditModel(IValidator<ClientEntity> validator)
        {
            _validator = validator;

            var factory = new ClientRepositoryFactory();
            _ClientRepository = factory.CreateRepository<ClientEntity>();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var tempClient = new ClientEntity { id = id };
            var found = await _ClientRepository.GetById(tempClient);
            if (found is null) return NotFound();
            Input = found;
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

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
                await _ClientRepository.Update(Input);
            }
            catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
            {
                ModelState.AddModelError("Input.email", "Ese email ya est� vinculado a otro cliente. Por favor, usa uno distinto.");
                return Page();
            }

            TempData["SuccessMessage"] = "Cliente actualizado correctamente.";
            return RedirectToPage("/Client/IndexClient");
        }
    }
}
