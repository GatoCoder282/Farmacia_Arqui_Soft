using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientEntity = Farmacia_Arqui_Soft.Domain.Models.Client;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Aplication.Services; 
using System;

namespace Farmacia_Arqui_Soft.Pages.Client
{
    public class IndexClientModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService; 

       
        public IEnumerable<ClientEntity> Clients { get; set; } = new List<ClientEntity>();

       
        public Dictionary<int, string> EncryptedIds { get; set; } = new Dictionary<int, string>();

        
        public IndexClientModel(IClientService clientService, IUserService userService, IEncryptionService encryptionService)
        {
            _clientService = clientService;
            _userService = userService;
            _encryptionService = encryptionService;
        }

        public async Task OnGetAsync()
        {
            Clients = await _clientService.ListAsync();

        
            foreach (var client in Clients)
            {
                var encryptedId = _encryptionService.EncryptId(client.id);
         
                EncryptedIds.Add(client.id, encryptedId);
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            const int ACTOR_ID = 1;

            try
            {
                await _clientService.SoftDeleteAsync(id, ACTOR_ID);
                TempData["SuccessMessage"] = $"Cliente eliminado correctamente."; 
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error al eliminar: {ex.Message}"; 
            }

            return RedirectToPage();
        }
    }
}