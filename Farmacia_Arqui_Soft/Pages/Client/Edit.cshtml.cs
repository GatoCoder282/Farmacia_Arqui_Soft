using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using MySql.Data.MySqlClient;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

using ClientEntity = Farmacia_Arqui_Soft.Domain.Models.Client;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Aplication.Services;
using System;
using System.Security.Cryptography;


namespace Farmacia_Arqui_Soft.Pages.Client
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly IRepository<ClientEntity> _ClientRepository;
        private readonly IValidator<ClientEntity> _validator;
        private readonly IEncryptionService _encryptionService;

        [BindProperty]
        public ClientEntity Input { get; set; } = new ClientEntity();

        public EditModel(IValidator<ClientEntity> validator, IEncryptionService encryptionService)
        {
            _validator = validator;

            var factory = new ClientRepositoryFactory();
            _ClientRepository = factory.CreateRepository<ClientEntity>();

            _encryptionService = encryptionService; 
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "ID de cliente no proporcionado.";
                return RedirectToPage("/Client/IndexClient");
            }

            int decryptedId;
            try
            {
                decryptedId = _encryptionService.DecryptId(id);
            }
            catch (FormatException)
            {
                TempData["Error"] = "Formato de ID inválido. Posible manipulación de URL.";
                return RedirectToPage("/Client/IndexClient");
            }
            catch (CryptographicException)
            {
                TempData["Error"] = "Error de seguridad. Posible manipulación de URL.";
                return RedirectToPage("/Client/IndexClient");
            }

            // Crear una entidad temporal para la búsqueda (asumiendo que GetById acepta una entidad)
            // NOTA: Si su IRepository<ClientEntity>.GetById(TEntity) solo acepta int, debe cambiar esta línea:
            // var found = await _ClientRepository.GetById(decryptedId);
            var tempClient = new ClientEntity { id = decryptedId };
            var found = await _ClientRepository.GetById(tempClient);

            if (found is null)
            {
                TempData["Error"] = $"Cliente no encontrado o eliminado.";
                return RedirectToPage("/Client/IndexClient");
            }

            Input = found;
            return Page();
        }

        // ✅ CORRECCIÓN: Se elimina [ValidateAntiForgeryToken] (Resuelve MVC1000)
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = _validator.Validate(Input);
            if (!result.IsSuccess)
            {
                foreach (var error in result.Errors)
                {

                    var field = error.Metadata.TryGetValue("field", out var f) ? f?.ToString() ?? string.Empty : string.Empty;
                    var key = field.StartsWith("Input.") ? field : $"Input.{field}";
                    ModelState.AddModelError(key, error.Message);
                }
                return Page();
            }

            try
            {
                // El campo Input.id (oculto en la vista Edit.cshtml) ya contiene el ID desencriptado.
                await _ClientRepository.Update(Input);
            }
            catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
            {
                ModelState.AddModelError("Input.email", "Ese email ya está vinculado a otro cliente. Por favor, usa uno distinto.");
                return Page();
            }

            TempData["Success"] = "Cliente actualizado correctamente.";
            return RedirectToPage("/Client/IndexClient");
        }
    }
}