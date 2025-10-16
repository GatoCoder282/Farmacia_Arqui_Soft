using System;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Domain.Ports; // 👈 Importar el servicio

namespace Farmacia_Arqui_Soft.Pages.Providers
{
    public class EditModel : PageModel
    {
        private readonly ProviderService _providerService;
        private readonly IEncryptionService _encryptionService; // 👈 Nuevo campo

        // 👈 Inyectar el IEncryptionService
        public EditModel(ProviderService providerService, IEncryptionService encryptionService)
        {
            _providerService = providerService;
            _encryptionService = encryptionService;
        }

        [BindProperty]
        public Provider Provider { get; set; } = new Provider();

        // 👈 Recibir el ID encriptado como string
        public async Task<IActionResult> OnGetAsync(string encryptedId)
        {
            if (string.IsNullOrEmpty(encryptedId))
            {
                TempData["ErrorMessage"] = "ID de proveedor no proporcionado.";
                return RedirectToPage("Index");
            }

            int id;
            try
            {
                // 👈 Desencriptar el ID antes de buscar el proveedor
                id = _encryptionService.DecryptId(encryptedId);
            }
            catch (FormatException)
            {
                TempData["ErrorMessage"] = "ID de proveedor inválido o corrupto.";
                return RedirectToPage("Index");
            }

            var existing = await _providerService.GetByIdAsync(id);

            if (existing == null)
            {
                TempData["ErrorMessage"] = "Proveedor no encontrado.";
                return RedirectToPage("Index");
            }

            Provider = existing;
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _providerService.UpdateAsync(Provider);
                TempData["SuccessMessage"] = "Proveedor actualizado correctamente.";
                return RedirectToPage("Index");
            }
            catch (ArgumentException ex)
            {
                foreach (var err in ex.Message.Split(','))
                {
                    ModelState.AddModelError(string.Empty, err.Trim());
                }
                return Page();
            }
        }
    }
}