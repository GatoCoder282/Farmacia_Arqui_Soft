using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Factory;
using ClientEntity = Farmacia_Arqui_Soft.Domain.Models.Client;
using Farmacia_Arqui_Soft.Repository;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class IndexClientModel : PageModel
    {
        private readonly IRepository<ClientEntity> _ClientRepository;
        public IEnumerable<ClientEntity> Clients { get; set; } = new List<ClientEntity>();

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
