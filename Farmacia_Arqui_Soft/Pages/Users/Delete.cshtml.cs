using Farmacia_Arqui_Soft.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Repository;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository<User> _userRepository;

        public DeleteModel(RepositoryFactory factory)
        {
            _userRepository = factory.CreateRepository<User>();
        }

        [BindProperty]
        public User User { get; set; } = new User();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var tempUser = new User { id = id };
            var userFromDb = await _userRepository.GetById(tempUser);

            if (userFromDb == null)
            {
                return RedirectToPage("Index");
            }

            User = userFromDb;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _userRepository.Delete(User);
            return RedirectToPage("Index");
        }
    }
}
