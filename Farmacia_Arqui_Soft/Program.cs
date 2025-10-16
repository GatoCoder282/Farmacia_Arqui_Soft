<<<<<<< HEAD
// Nuevos usings para auth y DI de servicios
using Farmacia_Arqui_Soft.Application.Services;
=======
using Farmacia_Arqui_Soft.Application.Services.UserServices;
>>>>>>> e613aa03f1683f7b1154163a08d1bff27455a6c0
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Domain.Ports.UserPorts;
using Farmacia_Arqui_Soft.Infraestructure.Data;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;
using Farmacia_Arqui_Soft.Validations.Clients;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Validations.Lots;
using Farmacia_Arqui_Soft.Validations.Providers;
using Farmacia_Arqui_Soft.Validations.Users;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Farmacia_Arqui_Soft
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ADO.NET singleton para conexi�n
            DatabaseConnection.Initialize(builder.Configuration);

            // -------------------- Infra: Factory & Repos --------------------
            // Mantengo ambas por compatibilidad con tus colegas
            builder.Services.AddSingleton<RepositoryFactory, UserRepositoryFactory>();
            builder.Services.AddSingleton<UserRepositoryFactory>();

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddScoped<IRepository<User>, UserRepository>();
            builder.Services.AddScoped<IRepository<Lot>, LotRepository>();
            builder.Services.AddScoped<IRepository<Provider>, ProviderRepository>();
            // Si tienes repos para Client/others, agr�galos igual

            // -------------------- Validadores --------------------
            builder.Services.AddScoped<IValidator<User>, UserValidator>();
            builder.Services.AddScoped<IValidator<Client>, ClientValidator>();
            builder.Services.AddScoped<IValidator<Lot>, LotValidator>();
            builder.Services.AddScoped<IValidator<Provider>, ProviderValidator>();

            // -------------------- Servicios de Dominio --------------------
            builder.Services.AddScoped<IUserService, UserService>();
<<<<<<< HEAD
           
=======
            // Hasher y generador de password
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IPasswordGenerator, CryptoPasswordGenerator>();

            // Pol�tica de username (pura l�gica de dominio)
            builder.Services.AddSingleton<IUsernamePolicy, UsernamePolicy>();
>>>>>>> e613aa03f1683f7b1154163a08d1bff27455a6c0

           

            // Email: implementaci�n de desarrollo que loguea a consola.
            // Cambia DevEmailSender por tu implementaci�n SMTP real cuando la tengas.
            builder.Services.AddScoped<IEmailSender, DevEmailSender>();

            // -------------------- Razor Pages --------------------
            builder.Services.AddRazorPages();

            // -------------------- Auth m�nima (para evitar tu excepci�n) --------------------
            // Si todav�a no usar�n [Authorize], igual deja esto para tener esquema por defecto.
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";           // crea esta page cuando integren login
                    options.AccessDeniedPath = "/Auth/Denied";   // y esta tambi�n
                });

            

            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            // -------------------- Pipeline --------------------
            // app.UseStaticFiles(); // Si usas archivos est�ticos cl�sicos, puedes habilitarlo
            app.UseRouting();

            app.UseAuthentication(); // IMPORTANTE: antes de UseAuthorization
            app.UseAuthorization();

            // Aspire/Static Assets helpers que ya usaban
            app.MapStaticAssets();
            app.MapRazorPages().WithStaticAssets();

            app.Run();
        }

        // -------------------- Implementaci�n DEV de IEmailSender --------------------
        // No rompe nada y te deja ver el "correo" en la consola de salida.
        // Reempl�zala por tu implementaci�n real (SMTP, API, etc.) cuando est� lista.
        internal sealed class DevEmailSender : IEmailSender
        {
            public Task SendAsync(string to, string subject, string body)
            {
                Console.WriteLine("=== DEV EMAIL SENDER ===");
                Console.WriteLine($"To: {to}");
                Console.WriteLine($"Subject: {subject}");
                Console.WriteLine(body);
                Console.WriteLine("========================");
                return Task.CompletedTask;
            }
        }
    }
}
