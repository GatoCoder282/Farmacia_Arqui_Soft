using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

using Farmacia_Arqui_Soft.Application.Services;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class CreateModel : PageModel
    {
        private readonly LotService _service;

        [BindProperty]
        public Lot Lot { get; set; } = new();

        public CreateModel(IValidator<Lot> validator)
        {
            _service = new LotService(validator);
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var (success, errors) = await _service.CreateAsync(Lot);
            if (!success && errors != null)
            {
                foreach (var error in errors)
                    ModelState.AddModelError($"Lot.{error.Key}", error.Value);
                return Page();
            }

            TempData["SuccessMessage"] = "Lote creado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
