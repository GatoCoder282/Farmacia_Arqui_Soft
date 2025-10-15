using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Application.Services;

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
                TempData["ErrorMessage"] = "Lote no encontrado.";
                return RedirectToPage("Index");
            }

            Lot = lote;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _service.DeleteAsync(Lot.Id);
            TempData["SuccessMessage"] = "Lote eliminado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
