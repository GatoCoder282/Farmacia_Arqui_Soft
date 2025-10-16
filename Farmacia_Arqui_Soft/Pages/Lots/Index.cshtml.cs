using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Domain.Ports; 

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class IndexModel : PageModel
    {
        private readonly LotService _service;
        private readonly IEncryptionService _encryptionService; 

        public IEnumerable<Lot> Lots { get; set; } = new List<Lot>();

        public IndexModel(IValidator<Lot> validator, IEncryptionService encryptionService)
        {
            _service = new LotService(validator);
            _encryptionService = encryptionService; 
        }

        public async Task OnGetAsync()
        {
            Lots = await _service.GetAllAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string encryptedId)
        {
            int id;
            try
            {
                id = _encryptionService.DecryptId(encryptedId);
            }
            catch (FormatException)
            {
                TempData["ErrorMessage"] = "ID de lote inválido o corrupto.";
                return RedirectToPage();
            }

            var success = await _service.SoftDeleteAsync(id);

            if (!success)
            {
                TempData["ErrorMessage"] = "Error al eliminar el lote. El lote no fue encontrado.";
                return RedirectToPage();
            }

            TempData["SuccessMessage"] = "Lote eliminado correctamente.";
            return RedirectToPage();
        }

    }
}