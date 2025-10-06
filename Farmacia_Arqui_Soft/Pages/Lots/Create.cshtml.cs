using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Factory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<Lot> _lotRepository;

        [BindProperty]
        public Lot Lot { get; set; } = new();

        public CreateModel()
        {
            var factory = new LotRepositoryFactory();
            _lotRepository = factory.CreateRepository<Lot>();
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _lotRepository.Create(Lot);
            return RedirectToPage("Index");
        }
    }
}
