using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Factory;

// Validaciones
using Farmacia_Arqui_Soft.Validations.Interfaces;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<Lot> _lotRepository;
        private readonly IValidator<Lot> _lotValidator;

        [BindProperty]
        public Lot Lot { get; set; } = new();

        // Mantiene Factory Method + inyecta validador
        public CreateModel(IValidator<Lot> lotValidator)
        {
            _lotValidator = lotValidator;

            var factory = new LotRepositoryFactory();
            _lotRepository = factory.CreateRepository<Lot>();
        }

        public void OnGet() { }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = _lotValidator.Validate(Lot);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    var key = error.Key.StartsWith("Lot.") ? error.Key : $"Lot.{error.Key}";
                    ModelState.AddModelError(key, error.Value);
                }
                return Page();
            }

            await _lotRepository.Create(Lot);
            TempData["Success"] = "Lote registrado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
