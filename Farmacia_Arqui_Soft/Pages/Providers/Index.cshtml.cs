using System.Collections.Generic;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Domain.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farmacia_Arqui_Soft.Pages.Providers
{
    public class IndexModel : PageModel
    {
        private readonly ProviderService _service;
        public IEnumerable<Provider> Providers { get; set; } = new List<Provider>();

        public IndexModel(ProviderService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            Providers = await _service.GetAllAsync();
        }
    }
}
