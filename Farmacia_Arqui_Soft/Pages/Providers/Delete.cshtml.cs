using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

namespace Farmacia_Arqui_Soft.Pages.Providers
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository<Provider> _repo;

        public DeleteModel()
        {
            var factory = new ProviderRepositoryFactory();
            _repo = factory.CreateRepository<Provider>();
        }

        [BindProperty]
        public Provider Provider { get; set; } = new Provider();


        public async Task<IActionResult> OnGetAsync(int id)
        {
            var tempProvider = new Provider { id = id };
            var userFromDb = await _repo.GetById(tempProvider);

            if (userFromDb == null)
            {
                return RedirectToPage("Index");
            }

            Provider = userFromDb;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _repo.Delete(Provider);
            return RedirectToPage("Index");
        }
        
    }
}
