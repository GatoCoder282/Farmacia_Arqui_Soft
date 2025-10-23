using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
// ✅ CLAVE: Alias de tipo
using ClientEntity = Farmacia_Arqui_Soft.Modules.ClientService.Domain.Client;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Modules.ClientService.Infrastructure.Persistence;

// ✅ CORRECCIÓN: Namespace consistente
namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class DetailsModel : PageModel
    {
        private readonly IRepository<ClientEntity> _ClientRepository;
        public ClientEntity Record { get; set; } // Usa ClientEntity

        public DetailsModel()
        {
            var factory = new ClientRepositoryFactory();
            _ClientRepository = factory.CreateRepository<ClientEntity>();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var tempClient = new ClientEntity { id = id }; // Usa ClientEntity
            var found = await _ClientRepository.GetById(tempClient);
            if (found is null) return NotFound();
            Record = found;
            return Page();
        }
    }
}