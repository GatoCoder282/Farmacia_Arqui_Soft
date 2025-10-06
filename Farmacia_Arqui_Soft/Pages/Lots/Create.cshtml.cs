using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Factory;
using Farmacia_Arqui_Soft.Interfaces;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<Lot> _repository;

        [BindProperty]
        public Lot Lot { get; set; } = new();

        public CreateModel()
        {
            var factory = new LotRepositoryFactory();
            _repository = factory.CreateRepository<Lot>();
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            await _repository.Create(Lot);
            return RedirectToPage("Index");
        }
    }
}
