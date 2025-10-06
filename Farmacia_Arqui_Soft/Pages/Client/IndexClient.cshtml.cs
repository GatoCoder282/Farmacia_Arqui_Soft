using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Factory;

// Alias para desambiguar: este "ClientEntity" es tu modelo
using ClientEntity = Farmacia_Arqui_Soft.Models.Client;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class IndexClientModel : PageModel
    {
        private readonly IRepository<ClientEntity> _clientRepository;

        public IEnumerable<ClientEntity> Clients { get; private set; } = new List<ClientEntity>();

        public IndexClientModel()
        {
            // Usa tu ClientRepositoryFactory existente
            var factory = new ClientRepositoryFactory();
            _clientRepository = factory.CreateRepository<ClientEntity>();
        }

        public async Task OnGetAsync()
        {
            Clients = await _clientRepository.GetAll();
        }
    }
}
