using Farmacia_Arqui_Soft.Aplication.Services;
using Farmacia_Arqui_Soft.Application.Services;
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

            // ADO.NET singleton para conexión
            DatabaseConnection.Initialize(builder.Configuration);

            // -------------------- Infra: Factory & Repos --------------------
            // Mantengo ambas por compatibilidad con tus colegas
            builder.Services.AddSingleton<RepositoryFactory, UserRepositoryFactory>();
            builder.Services.AddSingleton<UserRepositoryFactory>();
            // 💡 Agregamos la Factory de Cliente si piensas usarla en alguna parte
            builder.Services.AddSingleton<ClientRepositoryFactory>();

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // REPOSITORIOS (Adaptadores de Salida de la Arquitectura Hexagonal)
            builder.Services.AddScoped<IRepository<User>, UserRepository>();
            builder.Services.AddScoped<IRepository<Lot>, LotRepository>();
            builder.Services.AddScoped<IRepository<Provider>, ProviderRepository>();

            // 💡 REGISTRO DE REPOSITORIO DE CLIENTE (Necesario para ClientService)
            builder.Services.AddScoped<IRepository<Client>, ClientRepository>();


            // -------------------- Validadores --------------------
            builder.Services.AddScoped<IValidator<User>, UserValidator>();
            builder.Services.AddScoped<IValidator<Client>, ClientValidator>();
            builder.Services.AddScoped<IValidator<Lot>, LotValidator>();
            builder.Services.AddScoped<IValidator<Provider>, ProviderValidator>();

            // -------------------- Servicios de Aplicación (Puertos de Entrada) --------------------
            builder.Services.AddScoped<IUserService, UserService>();

            // ✅ CORRECCIÓN CRÍTICA: REGISTRO FALTANTE DEL SERVICIO DE CLIENTE
            // El contenedor de DI necesitaba esta línea para saber qué hacer cuando se pide IClientService.
            builder.Services.AddScoped<IClientService, ClientService>();

            // 💡 REGISTRO DE SERVICIO DE ENCRIPTACIÓN (Necesario para IndexClientModel y EditModel)
            builder.Services.AddSingleton<Farmacia_Arqui_Soft.Domain.Ports.IEncryptionService, Farmacia_Arqui_Soft.Aplication.Services.EncryptionService>();


            // Email: implementación de desarrollo que loguea a consola.
            builder.Services.AddScoped<IEmailSender, DevEmailSender>();

            // -------------------- Razor Pages --------------------
            builder.Services.AddRazorPages();

            // -------------------- Auth mínima --------------------
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.AccessDeniedPath = "/Auth/Denied";
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            // -------------------- Pipeline --------------------
            app.UseStaticFiles(); // Agrego si usas estáticos como CSS/JS/Imágenes
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Aspire/Static Assets helpers que ya usaban
            app.MapStaticAssets();
            app.MapRazorPages().WithStaticAssets();

            app.Run();
        }

        // -------------------- Implementación DEV de IEmailSender --------------------
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