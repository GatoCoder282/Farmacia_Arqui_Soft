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
        private readonly ICRUD<User> _userCrud;

        public IndexModel()
        {
            var factory = new CRUDFactory();
            _userCrud = factory.CreateCrud<User>();
        }

        public IEnumerable<User> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _userCrud.GetAll();
        }
    }
}

