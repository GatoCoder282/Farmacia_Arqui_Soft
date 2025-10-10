using Farmacia_Arqui_Soft.Factory;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repositories;
using Farmacia_Arqui_Soft.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farmacia_Arqui_Soft.Pages.Lots
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository<Lot> _lotRepository;

        [BindProperty]
        public Lot Lot { get; set; } = new();

        public DeleteModel()
        {
            var factory = new LotRepositoryFactory();
            _lotRepository = factory.CreateRepository<Lot>();
        }
        public async Task<IActionResult> OnGetAsync(int Id)
        {
            var tempLot = new Lot { Id = Id };
            var userFromDb = await _lotRepository.GetById(tempLot);

            if (userFromDb == null)
            {
                return RedirectToPage("Index");
            }

            Lot = userFromDb;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _lotRepository.Delete(Lot);
            return RedirectToPage("Index");
        }
        
    }
}
