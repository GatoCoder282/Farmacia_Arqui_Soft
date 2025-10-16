using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports; // 👈 Importar el servicio

namespace Farmacia_Arqui_Soft.Pages.Providers
{
    public class IndexModel : PageModel
    {
        private readonly ProviderService _service;
        private readonly IEncryptionService _encryptionService; // 👈 Nuevo campo

        public IEnumerable<Provider> Providers { get; set; } = new List<Provider>();

        // 👈 Inyectar el IEncryptionService
        public IndexModel(ProviderService service, IEncryptionService encryptionService)
        {
            _service = service;
            _encryptionService = encryptionService;
        }

        public async Task OnGetAsync()
        {
            Providers = await _service.GetAllAsync();
        }

        // 👈 Recibir el ID encriptado como string
        public async Task<IActionResult> OnPostDeleteAsync(string encryptedId)
        {
            int id;
            try
            {
                // 👈 Desencriptar el ID antes de pasarlo al servicio
                id = _encryptionService.DecryptId(encryptedId);
            }
            catch (System.FormatException)
            {
                TempData["ErrorMessage"] = "ID de proveedor inválido o corrupto.";
                return RedirectToPage();
            }

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