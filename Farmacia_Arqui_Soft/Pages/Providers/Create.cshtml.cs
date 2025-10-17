using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farmacia_Arqui_Soft.Pages.Providers
{
    public class CreateModel : PageModel
    {
        private readonly ProviderService _providerService;

        public CreateModel(ProviderService providerService)
        {
            _providerService = providerService;
        }

        [BindProperty]
        public Provider Provider { get; set; } = new Provider();

        public void OnGet()
        {
            // Este método se deja vacío intencionalmente, 
            // ya que la página de creación solo carga el formulario sin lógica adicional.
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            await _providerService.CreateAsync(Provider);
            return RedirectToPage("Index");
        }
    }
}
