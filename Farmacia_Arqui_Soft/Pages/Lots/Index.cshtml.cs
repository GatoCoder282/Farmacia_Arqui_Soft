using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class IndexModel : PageModel
    {
        private readonly LotService _lotService;

        public IEnumerable<Lot> Lots { get; set; } = new List<Lot>();

        public IndexModel(IValidator<Lot> validator)
        {
            _lotService = new LotService(validator);
        }

        public async Task OnGetAsync()
        {
            Lots = await _lotService.GetAllAsync();
        }
    }
}
