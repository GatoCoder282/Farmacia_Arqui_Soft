using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

namespace Farmacia_Arqui_Soft.Pages.Providers
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Provider> _repo;

        public IndexModel()
        {
            var factory = new ProviderRepositoryFactory();
            _repo = factory.CreateRepository<Provider>();
        }

        public IEnumerable<Provider> Providers { get; set; } = new List<Provider>();

        public async Task OnGetAsync()
        {
            Providers = await _repo.GetAll();
        }
    }
}
