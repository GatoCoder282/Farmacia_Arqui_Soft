using System;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farmacia_Arqui_Soft.Pages.Providers
{
    public class EditModel : PageModel
    {
        private readonly ProviderService _providerService;

        public EditModel(ProviderService providerService)
        {
            _providerService = providerService;
        }

        [BindProperty]
        public Provider Provider { get; set; } = new Provider();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var existing = await _providerService.GetByIdAsync(id);

            if (existing == null)
            {
                TempData["Error"] = "Proveedor no encontrado.";
                return RedirectToPage("Index");
            }

            Provider = existing;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _providerService.UpdateAsync(Provider);
                TempData["Success"] = "Proveedor actualizado correctamente.";
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
