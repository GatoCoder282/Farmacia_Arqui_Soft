using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Application.Services.UserServices;
using Farmacia_Arqui_Soft.Domain.Ports.UserPorts;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly IUserService _users;

        public DeleteModel(IUserService users)
        {
            _users = users;
        }

        [BindProperty] public int Id { get; set; }   // <- bindea el id del form
        public User? User { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var u = await _users.GetByIdAsync(id);
            if (u is null)
            {
                TempData["Error"] = "El usuario no existe.";
                return RedirectToPage("Index");
            }

            User = u;
            Id = id; // <- guardar para el POST
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                const int actorId = 1; // sin auth por ahora
                await _users.SoftDeleteAsync(Id, actorId);
                TempData["SuccessMessage"] = "Usuario eliminado.";
                return RedirectToPage("Index");
            }
            catch (NotFoundException)
            {
                TempData["ErrorMessage"] = "El usuario ya no existe.";
                return RedirectToPage("Index");
            }
        }
    }
}
