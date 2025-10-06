using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Factory;
using Farmacia_Arqui_Soft.Interfaces;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class EditModel : PageModel
    {
        private readonly IRepository<Lot> _repository;

        [BindProperty]
        public Lot Lot { get; set; } = new();

        public EditModel()
        {
            var factory = new LotRepositoryFactory();
            _repository = factory.CreateRepository<Lot>();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Lot = await _repository.GetById(id);
            if (Lot == null)
                return RedirectToPage("Index");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _repository.Update(Lot);
            return RedirectToPage("Index");
        }
    }
}
