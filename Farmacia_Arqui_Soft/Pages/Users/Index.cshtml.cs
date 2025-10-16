using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports; // ✅ Nuevo: Importar el puerto de encriptación
using System;
using System.Linq; // Agregado por si ListAsync devuelve IQueryable o similar

namespace Farmacia_Arqui_Soft.Pages.Users
{
    [Authorize(Roles = "Administrador")] // quítalo si no usas autenticación
    public class IndexModel : PageModel
    {
        private readonly IUserService _users;
        private readonly IEncryptionService _encryptionService; // ✅ Nuevo: Declarar servicio de encriptación

        public IEnumerable<User> Users { get; private set; } = new List<User>();

        // ✅ Nuevo: Diccionario para guardar el ID encriptado, clave=ID numérico, valor=ID encriptado
        public Dictionary<int, string> EncryptedIds { get; private set; } = new Dictionary<int, string>();


        // ✅ Modificado: Inyección de IEncryptionService
        public IndexModel(IUserService users, IEncryptionService encryptionService)
        {
            _users = users;
            _encryptionService = encryptionService; // ✅ Asignación del servicio
        }

        public async Task OnGetAsync()
        {
            Users = await _users.ListAsync();

            // ✅ Nuevo: Encriptar los IDs de los usuarios
            foreach (var user in Users)
            {
                var encryptedId = _encryptionService.EncryptId(user.id);
                EncryptedIds.Add(user.id, encryptedId);
            }
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