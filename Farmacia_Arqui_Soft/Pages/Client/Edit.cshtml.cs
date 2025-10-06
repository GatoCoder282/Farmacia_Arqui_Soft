using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Factory;
using System.Threading.Tasks;
using ClientEntity = Farmacia_Arqui_Soft.Models.Client;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class EditModel : PageModel
    {
        private readonly IRepository<ClientEntity> _repo;

        [BindProperty]
        public ClientEntity Input { get; set; }

        public EditModel()
        {
            var factory = new ClientRepositoryFactory();
            _repo = factory.CreateRepository<ClientEntity>();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var found = await _repo.GetById(id);
            if (found is null) return NotFound();
            Input = found;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _repo.Update(Input);

            return RedirectToPage("/Client/IndexClient");
        }
    }
}
