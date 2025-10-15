using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;     // <- si vas a proteger la página
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports.UserPorts;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    [Authorize(Roles = "Administrador")] // quítalo si aún no tienes auth funcionando
    public class IndexModel : PageModel
    {
        private readonly IUserService _users;

        public IndexModel(IUserService users)
        {
            _users = users;
        }

        public IEnumerable<User> Users { get; private set; } = new List<User>();

        public async Task OnGetAsync()
        {
            Users = await _users.ListAsync(); // ya viene filtrado por is_deleted en tu repo
        }
    }
}
