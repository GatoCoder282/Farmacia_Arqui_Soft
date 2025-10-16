using Farmacia_Arqui_Soft.Application.DTOs;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IUserService _users;
        private readonly IEncryptionService _encryptionService;

        public EditModel(IUserService users, IEncryptionService encryptionService)
        {
            _users = users;
            _encryptionService = encryptionService;
        }

        [BindProperty]
        public UserEditVm Input { get; set; } = new();

        public SelectList Roles { get; private set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            LoadRoles();

            int userId;

            try
            {

                userId = _encryptionService.DecryptId(id);
            }
            catch (FormatException)
            {
                
                TempData["ErrorMessage"] = "El enlace de edición es inválido o ha sido modificado.";
                return RedirectToPage("Index");
            }
            catch (CryptographicException)
            {
                
                TempData["ErrorMessage"] = "Error de seguridad. El ID no pudo ser desencriptado.";
                return RedirectToPage("Index");
            }

            var u = await _users.GetByIdAsync(userId);
            if (u is null) return RedirectToPage("Index");

            Input = new UserEditVm
            {
                Id = u.id,
                Username = u.username,
                FirstName = u.first_name,
                SecondName = u.second_name,
                LastName = u.last_name,
                Mail = u.mail ?? "",
                Phone = u.phone,
                Ci = u.ci,
                Role = u.role,
                IsActive = !u.is_deleted 
            };

            return Page();
        }

        
        public async Task<IActionResult> OnPostAsync()
        {
    
            const int ACTOR_ID = 1;

            if (!ModelState.IsValid)
            {
                LoadRoles();
                return Page();
            }

            try
            {
                
                var dto = new UserUpdateDto(
                    
                    Input.FirstName,
                    Input.SecondName,
                    Input.LastName,
                    Input.Mail,
                    Input.Phone,
                    Input.Ci,
                    Input.Role,
                    Input.Password
                );

                await _users.UpdateAsync(Input.Id, dto, ACTOR_ID);

                TempData["SuccessMessage"] = $"Usuario '{Input.Username}' actualizado correctamente.";
                return RedirectToPage("Index");
            }
            
            catch (Application.Services.ValidationException vex)
            {
                foreach (var kv in vex.Errors)
                    ModelState.AddModelError(kv.Key ?? string.Empty, kv.Value);
                return Page();
            }
            
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                LoadRoles();
                return Page();
            }
        }


        private void LoadRoles()
        {
            Roles = new SelectList(Enum.GetValues(typeof(UserRole)));
        }

        public class UserEditVm
        {
            [HiddenInput] public int Id { get; set; }

            [Display(Name = "Usuario")] public string Username { get; set; } = "";

            [Required, Display(Name = "Nombre")] public string FirstName { get; set; } = "";
            [Display(Name = "Segundo nombre")] public string? SecondName { get; set; }
            [Required, Display(Name = "Apellido")] public string LastName { get; set; } = "";

            [Required, EmailAddress, Display(Name = "Correo")] public string Mail { get; set; } = "";

            [Required, Range(100000, 9999999999), Display(Name = "Teléfono")]
            public int Phone { get; set; }

            [Required, Display(Name = "CI")] public string Ci { get; set; } = "";

            [Required, Display(Name = "Rol")] public UserRole Role { get; set; } = UserRole.Cajero;

            [MinLength(4), DataType(DataType.Password), Display(Name = "Nueva contraseña")]
            public string? Password { get; set; }
            public bool IsActive { get; internal set; }
        }
    }
}