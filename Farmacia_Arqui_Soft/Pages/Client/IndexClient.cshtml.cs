using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Factory;
using ClientEntity = Farmacia_Arqui_Soft.Models.Client;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class IndexClientModel : PageModel
    {
        private readonly IRepository<ClientEntity> _ClientRepository;
        public IEnumerable<ClientEntity> Clients { get; private set; } = new List<ClientEntity>();

        public IndexClientModel()
        {
            var factory = new ClientRepositoryFactory();
            _ClientRepository = factory.CreateRepository<ClientEntity>();
        }

        public async Task OnGetAsync()
        {
            Clients = await _ClientRepository.GetAll();
        }
    }
}
