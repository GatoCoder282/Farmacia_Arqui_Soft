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
    public class CreateModel : PageModel
    {
        private readonly IRepository<ClientEntity> _ClientRepository;
        private readonly IValidator<ClientEntity> _validator;

        [BindProperty]
        public ClientEntity Input { get; set; } = new ClientEntity { };

        public CreateModel(IValidator<ClientEntity> validator)
        {
            _validator = validator;

            var factory = new ClientRepositoryFactory();
            _ClientRepository = factory.CreateRepository<ClientEntity>();
        }

        public void OnGet()
        {
            // Este método se deja vacío intencionalmente, 
            // ya que la página de creación solo carga el formulario sin lógica adicional.
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = _validator.Validate(Input);
            if (!result.IsSuccess)
            {
                foreach (var error in result.Errors)
                {
                    var field = error.Metadata.TryGetValue("field", out var f) ? f?.ToString() : string.Empty;
                    var key = field.StartsWith("Input.") ? field : $"Input.{field}";
                    ModelState.AddModelError(key, error.Message);
                }
                return Page();
            }

            try
            {
                await _ClientRepository.Create(Input);
            }
            catch (MySqlException ex) when (ex.Number == 1062) 
            {
                ModelState.AddModelError("Input.email", "Ese email ya se encuentra vinculado a otro cliente. Por favor, usa uno distinto.");
                return Page();
            }

            TempData["SuccessMessage"] = "Cliente creado correctamente.";
            return RedirectToPage("/Client/IndexClient");
        }
    }
}
