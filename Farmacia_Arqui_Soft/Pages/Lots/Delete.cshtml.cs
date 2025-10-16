using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Domain.Models;

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
            {
                TempData["ErrorMessage"] = "El lote no fue encontrado o ya fue eliminado.";
                return RedirectToPage("Index");
            }

            Lot = lote;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 🔹 Simulamos el ID del usuario autenticado (hasta tener auth real)
            const int actorId = 1;

            var success = await _service.SoftDeleteAsync(Lot.Id, actorId);
            if (!success)
            {
                TempData["ErrorMessage"] = "Error al eliminar el lote. Puede que no exista o ya esté eliminado.";
                return RedirectToPage("Index");
            }

            TempData["SuccessMessage"] = "Lote eliminado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
