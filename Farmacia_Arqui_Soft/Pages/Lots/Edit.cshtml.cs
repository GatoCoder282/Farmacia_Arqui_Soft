using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Factory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class EditModel : PageModel
    {
        private readonly IRepository<Lot> _lotRepository;

        [BindProperty]
        public Lot Lot { get; set; } = new();

        public EditModel()
        {
            var factory = new LotRepositoryFactory();
            _lotRepository = factory.CreateRepository<Lot>();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var lote = await _lotRepository.GetById(id);
            if (lote == null)
                return NotFound();

            Lot = lote;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _lotRepository.Update(Lot);
            return RedirectToPage("Index");
        }
    }
}
