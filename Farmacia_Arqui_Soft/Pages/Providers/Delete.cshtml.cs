using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repositories;
using Farmacia_Arqui_Soft.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            var p = await _repo.GetById(id);
            if (p == null) return RedirectToPage("Index");

            Provider = p;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _repo.Delete(Provider.id); 
            return RedirectToPage("Index");
        }
    }
}
