using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Validations.Interfaces;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class IndexModel : PageModel
    {
        private readonly LotService _service;

        public IEnumerable<Lot> Lots { get; set; } = new List<Lot>();

        public IndexModel(IValidator<Lot> validator)
        {
            _service = new LotService(validator);
        }

        public async Task OnGetAsync()
        {
            Lots = await _service.GetAllAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var success = await _service.SoftDeleteAsync(id); 

            if (!success)
            {
                TempData["ErrorMessage"] = "Error al eliminar el lote.";
                return RedirectToPage();
            }

            TempData["SuccessMessage"] = "Lote eliminado correctamente.";
            return RedirectToPage();
        }

    }
}
