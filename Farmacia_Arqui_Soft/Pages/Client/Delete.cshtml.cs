using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Factory;
using System.Threading.Tasks;
using ClientEntity = Farmacia_Arqui_Soft.Models.Client;
using Farmacia_Arqui_Soft.Repository;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository<ClientEntity> _ClientRepository;

        public ClientEntity Record { get; private set; }

        public DeleteModel()
        {
            var factory = new ClientRepositoryFactory();
            _ClientRepository = factory.CreateRepository<ClientEntity>();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var tempClient = new ClientEntity { id = id };
            var userFromDb = await _ClientRepository.GetById(tempClient);

            if (userFromDb == null)
            {
                return RedirectToPage("Index");
            }

            Record = userFromDb;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _ClientRepository.Delete(Record);
            return RedirectToPage("Index");
        }

    }
}
