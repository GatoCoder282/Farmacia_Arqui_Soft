using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class EditModel : PageModel
    {
        private readonly LotService _service;

        [BindProperty]
        public Lot Lot { get; set; } = new();

        public EditModel(IValidator<Lot> validator)
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
            var (success, errors) = await _service.UpdateAsync(Lot);
            if (!success && errors != null)
            {
                foreach (var error in errors)
                    ModelState.AddModelError($"Lot.{error.Key}", error.Value);
                return Page();
            }

            return RedirectToPage("/Shared/Success", new { message = "Lote actualizado correctamente" });
        }
    }
}
