using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Factory;
using System.Threading.Tasks;

// Alias para el modelo Client
using ClientEntity = Farmacia_Arqui_Soft.Models.Client;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<ClientEntity> _repo;

        [BindProperty]
        public ClientEntity Input { get; set; } = new ClientEntity { };

        public CreateModel()
        {
            var factory = new ClientRepositoryFactory();
            _repo = factory.CreateRepository<ClientEntity>();
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Auditoría mínima
            await _repo.Create(Input);

            return RedirectToPage("/Client/IndexClient");
        }
    }
}
