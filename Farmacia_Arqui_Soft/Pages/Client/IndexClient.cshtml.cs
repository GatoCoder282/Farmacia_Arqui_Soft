using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientEntity = Farmacia_Arqui_Soft.Domain.Models.Client;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Aplication.Services; // Asegúrate de tener este using si EncryptionService está allí
using System;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class IndexClientModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService; // Inyección para encriptar

        // Propiedad original de la vista
        public IEnumerable<ClientEntity> Clients { get; set; } = new List<ClientEntity>();

        // Diccionario para almacenar solo los IDs encriptados que necesita la vista (Editar)
        public Dictionary<int, string> EncryptedIds { get; set; } = new Dictionary<int, string>();

        // ✅ CORRECCIÓN: Inyección de todos los servicios
        public IndexClientModel(IClientService clientService, IUserService userService, IEncryptionService encryptionService)
        {
            _clientService = clientService;
            _userService = userService;
            _encryptionService = encryptionService;
        }

        public async Task OnGetAsync()
        {
            Clients = await _clientService.ListAsync();

            // Llenar el diccionario de IDs encriptados
            foreach (var client in Clients)
            {
                var encryptedId = _encryptionService.EncryptId(client.id);
                // La clave es el ID numérico, el valor es el ID encriptado
                EncryptedIds.Add(client.id, encryptedId);
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            const int ACTOR_ID = 1;

            try
            {
                await _clientService.SoftDeleteAsync(id, ACTOR_ID);
                TempData["SuccessMessage"] = $"Cliente eliminado correctamente."; // Usar la clave de TempData de la vista
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error al eliminar: {ex.Message}"; // Usar la clave de TempData de la vista
            }

            return RedirectToPage();
        }
    }
}