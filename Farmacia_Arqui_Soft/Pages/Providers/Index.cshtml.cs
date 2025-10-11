using System.Collections.Generic;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Repository;
using Farmacia_Arqui_Soft.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;

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
