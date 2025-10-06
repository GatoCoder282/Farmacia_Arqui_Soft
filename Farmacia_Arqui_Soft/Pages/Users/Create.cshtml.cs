using Farmacia_Arqui_Soft.Factory;
using Farmacia_Arqui_Soft.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Interfaces;
using System.Threading.Tasks;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<User> _userRepository;

        public CreateModel(RepositoryFactory factory)
        {
            _userRepository = factory.CreateRepository<User>();
        }

        [BindProperty]
        public User User { get; set; } = new User();

        public void OnGet()
        {
            // Nada que hacer en Get
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userRepository.Create(User);
            return RedirectToPage("Index");
        }
    }
}
