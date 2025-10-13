using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Domain.Models;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class EditModel : PageModel
    {
        private readonly LotService _lotService;

        [BindProperty]
        public Lot Lot { get; set; } = new();

        public EditModel(IValidator<Lot> validator)
        {
            _lotService = new LotService(validator);
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var lote = await _lotService.GetByIdAsync(id);
            if (lote == null)
                return NotFound();

            Lot = lote;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var (success, errors) = await _lotService.UpdateAsync(Lot);

            if (!success && errors != null)
            {
                foreach (var error in errors)
                    ModelState.AddModelError($"Lot.{error.Key}", error.Value);
                return Page();
            }

            TempData["Success"] = "Lote actualizado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
