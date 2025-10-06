using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Factory;
using System.Threading.Tasks;
using ClientEntity = Farmacia_Arqui_Soft.Models.Client;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository<ClientEntity> _repo;

        public ClientEntity Record { get; private set; }

        public DeleteModel()
        {
            var factory = new ClientRepositoryFactory();
            _repo = factory.CreateRepository<ClientEntity>();
        }

        // Carga el registro para confirmar
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var found = await _repo.GetById(id);
            if (found is null) return NotFound();
            Record = found;
            return Page();
        }

        // Confirma la eliminación
        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _repo.Delete(id);
            return RedirectToPage("/Client/IndexClient");
        }
    }
}
