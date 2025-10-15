using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Application.Services.UserServices;
using Farmacia_Arqui_Soft.Application.DTOs;
using Farmacia_Arqui_Soft.Domain.Ports.UserPorts;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IUserService _users;

        public EditModel(IUserService users)
        {
            _users = users;
        }

        [BindProperty]
        public UserEditVm Input { get; set; } = new();

        public SelectList Roles { get; private set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            LoadRoles();

            var u = await _users.GetByIdAsync(id);
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
                Role = u.role
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LoadRoles();
            if (!ModelState.IsValid) return Page();

            try
            {
                // Usa par�metros con nombre para no depender del orden del record
                var dto = new UserUpdateDto(
                    FirstName: Input.FirstName,
                    SecondName: Input.SecondName,
                    LastName: Input.LastName,
                    Mail: Input.Mail,
                    Phone: Input.Phone,
                    Ci: Input.Ci,
                    Role: Input.Role,
                    Password: string.IsNullOrWhiteSpace(Input.Password) ? null : Input.Password
                );

                const int actorId = 1;
                await _users.UpdateAsync(Input.Id, dto, actorId);

                TempData["Success"] = "Usuario actualizado.";
                return RedirectToPage("Index");
            }
            catch (NotFoundException)
            {
                TempData["Error"] = "El usuario ya no existe.";
                return RedirectToPage("Index");
            }
            catch (Application.Services.UserServices.ValidationException vex)
            {
                foreach (var kv in vex.Errors)
                    ModelState.AddModelError(kv.Key ?? string.Empty, kv.Value);
                return Page();
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
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

            [Required, Range(100000, 9999999999), Display(Name = "Tel�fono")]
            public int Phone { get; set; }

            [Required, Display(Name = "CI")] public string Ci { get; set; } = "";

            [Required, Display(Name = "Rol")] public UserRole Role { get; set; } = UserRole.Cajero;

            [MinLength(4), DataType(DataType.Password), Display(Name = "Nueva contrase�a")]
            public string? Password { get; set; }
        }
    }
}
