using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Domain.Models;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class DeleteModel : PageModel
    {
        private readonly LotService _lotService;

        [BindProperty]
        public Lot Lot { get; set; } = new();

        public DeleteModel(IValidator<Lot> validator)
        {
            _lotService = new LotService(validator);
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var lote = await _lotService.GetByIdAsync(id);
            if (lote == null)
                return RedirectToPage("Index");

            Lot = lote;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _lotService.DeleteAsync(Lot.Id);
            TempData["Success"] = "Lote eliminado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
