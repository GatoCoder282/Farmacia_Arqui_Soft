// Nuevos usings para auth y DI de servicios
using Farmacia_Arqui_Soft.Application.Services.UserServices;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Domain.Services;
using Farmacia_Arqui_Soft.Infraestructure.Data;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;
using Farmacia_Arqui_Soft.Infraestructure.Security;
using Farmacia_Arqui_Soft.Validations.Clients;
// Validations
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

            // ADO.NET singleton para conexión
            DatabaseConnection.Initialize(builder.Configuration);

            // -------------------- Infra: Factory & Repos --------------------
            // Mantengo ambas por compatibilidad con tus colegas
            builder.Services.AddSingleton<RepositoryFactory, UserRepositoryFactory>();
            builder.Services.AddSingleton<UserRepositoryFactory>();

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddScoped<IRepository<User>, UserRepository>();
            builder.Services.AddScoped<IRepository<Lot>, LotRepository>();
            builder.Services.AddScoped<IRepository<Provider>, ProviderRepository>();
            // Si tienes repos para Client/others, agrégalos igual

            // -------------------- Validadores --------------------
            builder.Services.AddScoped<IValidator<User>, UserValidator>();
            builder.Services.AddScoped<IValidator<Client>, ClientValidator>();
            builder.Services.AddScoped<IValidator<Lot>, LotValidator>();
            builder.Services.AddScoped<IValidator<Provider>, ProviderValidator>();

            // -------------------- Servicios de Dominio --------------------
            builder.Services.AddScoped<IUserService, UserService>();
            // Hasher y generador de password
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IPasswordGenerator, CryptoPasswordGenerator>();

            // Política de username (pura lógica de dominio)
            builder.Services.AddSingleton<IUsernamePolicy, UsernamePolicy>();

           

            // Email: implementación de desarrollo que loguea a consola.
            // Cambia DevEmailSender por tu implementación SMTP real cuando la tengas.
            builder.Services.AddScoped<IEmailSender, DevEmailSender>();

            // -------------------- Razor Pages --------------------
            builder.Services.AddRazorPages();

            // -------------------- Auth mínima (para evitar tu excepción) --------------------
            // Si todavía no usarán [Authorize], igual deja esto para tener esquema por defecto.
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";           // crea esta page cuando integren login
                    options.AccessDeniedPath = "/Auth/Denied";   // y esta también
                });

            

            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            // -------------------- Pipeline --------------------
            // app.UseStaticFiles(); // Si usas archivos estáticos clásicos, puedes habilitarlo
            app.UseRouting();

            app.UseAuthentication(); // IMPORTANTE: antes de UseAuthorization
            app.UseAuthorization();

            // Aspire/Static Assets helpers que ya usaban
            app.MapStaticAssets();
            app.MapRazorPages().WithStaticAssets();

            app.Run();
        }

        // -------------------- Implementación DEV de IEmailSender --------------------
        // No rompe nada y te deja ver el "correo" en la consola de salida.
        // Reemplázala por tu implementación real (SMTP, API, etc.) cuando esté lista.
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
