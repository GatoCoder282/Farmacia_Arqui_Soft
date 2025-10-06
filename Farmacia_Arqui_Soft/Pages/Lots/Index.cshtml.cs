using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Factory;
using Farmacia_Arqui_Soft.Interfaces;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Lot> _repository;
        public IEnumerable<Lot> Lots { get; set; } = new List<Lot>();

        public IndexModel()
        {
            var factory = new LotRepositoryFactory();
            _repository = factory.CreateRepository<Lot>();
        }

        public async Task OnGetAsync()
        {
            Lots = await _repository.GetAll();
        }
    }
}
