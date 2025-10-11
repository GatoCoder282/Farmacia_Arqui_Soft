using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Factory;
using Farmacia_Arqui_Soft.Repository;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Lot> _lotRepository;

        public IEnumerable<Lot> Lots { get; set; } = new List<Lot>();

        public IndexModel()
        {
            var factory = new LotRepositoryFactory();
            _lotRepository = factory.CreateRepository<Lot>();
        }

        public async Task OnGetAsync()
        {
            Lots = await _lotRepository.GetAll();
        }
    }
}
