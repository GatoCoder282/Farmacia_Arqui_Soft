using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

namespace Farmacia_Arqui_Soft.Pages.Providers
{
    public class EditModel : PageModel
    {
        private readonly IRepository<Provider> _repo;
        private readonly IValidator<Provider> _validator;

        public EditModel(IValidator<Provider> validator)
        {
            _validator = validator;
            var factory = new ProviderRepositoryFactory();
            _repo = factory.CreateRepository<Provider>();
        }

        [BindProperty]
        public Provider Provider { get; set; } = new Provider();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var tempProvider = new Provider { id = id };
            var existing = await _repo.GetById(tempProvider);
            if (existing == null) return RedirectToPage("Index");

            Provider = existing;
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = _validator.Validate(Provider);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors)
                {
                    var key = err.Key.StartsWith("Provider.") ? err.Key : $"Provider.{err.Key}";
                    ModelState.AddModelError(key, err.Value);
                }
                return Page();
            }

            await _repo.Update(Provider);
            TempData["Success"] = "Proveedor actualizado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
