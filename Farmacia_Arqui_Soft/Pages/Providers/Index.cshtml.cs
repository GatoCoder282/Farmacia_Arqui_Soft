using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Domain.Models;

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


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                TempData["SuccessMessage"] = "Proveedor eliminado correctamente.";
            }
            catch (System.Exception)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el proveedor.";
            }
            return RedirectToPage();
        }
    }
}
