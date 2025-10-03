using Farmacia_Arqui_Soft.Factory;
using Farmacia_Arqui_Soft.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Interfaces;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<User> _userRepository;

        public IndexModel(RepositoryFactory factory)
        {
            _userRepository = factory.CreateRepository<User>();
        }

        public IEnumerable<User> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _userRepository.GetAll();
        }
    }
}
