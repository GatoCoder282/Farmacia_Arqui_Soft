using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Application.Services;
using Farmacia_Arqui_Soft.Application.DTOS;
using Farmacia_Arqui_Soft.Domain.Ports;

namespace Farmacia_Arqui_Soft.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _users;

        public CreateModel(IUserService users)
        {
            _users = users;
        }

        [BindProperty]
        public UserCreateVm Input { get; set; } = new();

        public SelectList Roles { get; private set; } = default!;

        public void OnGet()
        {
            LoadRoles();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            LoadRoles();
            if (!ModelState.IsValid) return Page();

            try
            {
                var dto = new UserCreateDto(
                    FirstName: Input.FirstName,
                    SecondName: string.IsNullOrWhiteSpace(Input.SecondName) ? null : Input.SecondName,
                    LastName: Input.LastName,
                    Mail: Input.Mail,
                    Phone: Input.Phone,
                    Ci: Input.Ci,
                    Role: Input.Role
                );

                const int actorId = 1; // sin auth por ahora
                await _users.RegisterAsync(dto, actorId);

                TempData["Success"] = "Usuario creado correctamente. Se envió una contraseña temporal al correo.";
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
                return Page();
            }
        }

        private void LoadRoles()
        {
            Roles = new SelectList(Enum.GetValues(typeof(UserRole)));
        }

        public class UserCreateVm
        {
            [Required, Display(Name = "Nombre")]
            public string FirstName { get; set; } = "";

            [Display(Name = "Segundo nombre")]
            public string? SecondName { get; set; }

            [Required, Display(Name = "Apellido")]
            public string LastName { get; set; } = "";

            [Required, EmailAddress, Display(Name = "Correo")]
            public string Mail { get; set; } = "";

            [Required, Range(100000, 9999999999), Display(Name = "Teléfono")]
            public int Phone { get; set; }

            [Required, Display(Name = "CI")]
            public string Ci { get; set; } = "";

            [Required, Display(Name = "Rol")]
            public UserRole Role { get; set; } = UserRole.Cajero;
        }
    }
}
