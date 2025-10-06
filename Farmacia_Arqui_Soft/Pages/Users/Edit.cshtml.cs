using Farmacia_Arqui_Soft.Factory;
using Farmacia_Arqui_Soft.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Interfaces;
using System.Threading.Tasks;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IRepository<User> _userRepository;

        public EditModel(RepositoryFactory factory)
        {
            _userRepository = factory.CreateRepository<User>();
        }

        [BindProperty]
        public User User { get; set; } = new User();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userFromDb = await _userRepository.GetById(id);
            if (userFromDb == null)
            {
                return RedirectToPage("Index"); // Si no existe, regresar al listado
            }

            User = userFromDb;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userRepository.Update(User);
            return RedirectToPage("Index");
        }
    }
}
