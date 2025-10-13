using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class DeleteModel : PageModel
    {
        private readonly LotService _service;

        [BindProperty]
        public Lot Lot { get; set; } = new();

        public DeleteModel(IValidator<Lot> validator)
        {
            _service = new LotService(validator);
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var lote = await _service.GetByIdAsync(id);
            if (lote == null)
                return RedirectToPage("/Shared/Error", new { message = "Lote no encontrado" });

            Lot = lote;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var deleted = await _service.DeleteAsync(Lot.Id);
            if (!deleted)
                return RedirectToPage("/Shared/Error", new { message = "Error al eliminar lote" });

            return RedirectToPage("/Shared/Success", new { message = "Lote eliminado correctamente" });
        }
    }
}
