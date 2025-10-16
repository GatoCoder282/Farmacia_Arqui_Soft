using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Domain.Ports; 

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    // 👈 Cambiar el patrón de ruta para aceptar cualquier string (el ID encriptado)
    // El patrón "{id}" sin ":int" acepta cualquier string.
    [Route("Lots/Edit/{encryptedId?}")]
    public class EditModel : PageModel
    {
        private readonly LotService _service;
        private readonly IEncryptionService _encryptionService;

        [BindProperty]
        public Lot Lot { get; set; } = new();

        
        public EditModel(IValidator<Lot> validator, IEncryptionService encryptionService)
        {
            _service = new LotService(validator);
            _encryptionService = encryptionService; 
        }

        public async Task<IActionResult> OnGetAsync(string encryptedId)
        {
            if (string.IsNullOrEmpty(encryptedId))
            {
                TempData["ErrorMessage"] = "ID de lote no proporcionado.";
                return RedirectToPage("Index");
            }

            int id;
            try
            {
                id = _encryptionService.DecryptId(encryptedId);
            }
            catch (FormatException)
            {
                TempData["ErrorMessage"] = "ID de lote inválido o corrupto.";
                return RedirectToPage("Index");
            }

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
            

            if (!ModelState.IsValid) return Page();

            var (success, errors) = await _service.UpdateAsync(Lot);
            if (!success && errors != null)
            {
                foreach (var error in errors)
                    ModelState.AddModelError($"Lot.{error.Key}", error.Value);
                return Page();
            }

            TempData["SuccessMessage"] = "Lote actualizado correctamente.";
            return RedirectToPage("Index");
        }
    }
}