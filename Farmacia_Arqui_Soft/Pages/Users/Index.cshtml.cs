using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    [Authorize(Roles = "Administrador")] // quítalo si no usas autenticación
    public class IndexModel : PageModel
    {
        private readonly IUserService _users;

        public IEnumerable<User> Users { get; private set; } = new List<User>();

        public IndexModel(IUserService users)
        {
            _users = users;
        }

        public async Task OnGetAsync()
        {
            Users = await _users.ListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            const int ACTOR_ID = 1;

            try
            {
                await _users.SoftDeleteAsync(id, ACTOR_ID);
                TempData["SuccessMessage"] = $"Usuario eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el usuario: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
