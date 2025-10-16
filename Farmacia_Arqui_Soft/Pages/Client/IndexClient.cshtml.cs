using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientEntity = Farmacia_Arqui_Soft.Domain.Models.Client;
using Farmacia_Arqui_Soft.Domain.Ports;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class IndexClientModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IUserService _userService;

        public IEnumerable<ClientEntity> Clients { get; set; } = new List<ClientEntity>();

        public IndexClientModel(IClientService clientService, IUserService userService)
        {
            _clientService = clientService;
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            Clients = await _clientService.ListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            const int ACTOR_ID = 1;

            try
            {
                await _clientService.SoftDeleteAsync(id, ACTOR_ID);
                TempData["Success"] = $"Cliente eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error al eliminar: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
