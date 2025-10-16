using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farmacia_Arqui_Soft.Pages.Providers
{
    public class DeleteModel : PageModel
    {
        private readonly ProviderService _service;

        public DeleteModel(ProviderService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToPage("Index");
        }
    }
}
