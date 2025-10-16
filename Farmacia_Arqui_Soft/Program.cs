using System.Globalization;
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
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace Farmacia_Arqui_Soft
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -------------------- Conexión DB --------------------
            DatabaseConnection.Initialize(builder.Configuration);

            // -------------------- Repositorios --------------------
            builder.Services.AddSingleton<RepositoryFactory, UserRepositoryFactory>();
            builder.Services.AddSingleton<UserRepositoryFactory>();
            builder.Services.AddSingleton<ClientRepositoryFactory>();
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddScoped<IRepository<User>, UserRepository>();
            builder.Services.AddScoped<IRepository<Lot>, LotRepository>();
            builder.Services.AddScoped<IRepository<Provider>, ProviderRepository>();
            builder.Services.AddScoped<IRepository<Client>, ClientRepository>();

            // -------------------- Servicios --------------------
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<Farmacia_Arqui_Soft.Application.Services.ProviderService>();
            builder.Services.AddSingleton<Farmacia_Arqui_Soft.Domain.Ports.IEncryptionService, EncryptionService>();
            builder.Services.AddScoped<IEmailSender, DevEmailSender>();

            // -------------------- Validadores --------------------
            builder.Services.AddScoped<IValidator<User>, UserValidator>();
            builder.Services.AddScoped<IValidator<Client>, ClientValidator>();
            builder.Services.AddScoped<IValidator<Lot>, LotValidator>();
            builder.Services.AddScoped<IValidator<Provider>, ProviderValidator>();

            // -------------------- Razor Pages --------------------
            builder.Services.AddRazorPages()
                .AddViewOptions(o =>
                {
                    // Desactivar validación del lado del cliente (HTML5/jQuery)
                    o.HtmlHelperOptions.ClientValidationEnabled = false;
                })
                .AddMvcOptions(options =>
                {
                    // Desactivar DataAnnotations: solo validaciones personalizadas
                    options.ModelMetadataDetailsProviders.Clear();
                    options.ModelValidatorProviders.Clear();
                });

            // -------------------- Localización (opcional) --------------------
            var supportedCultures = new[] { new CultureInfo("es-BO"), new CultureInfo("es") };
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("es-BO");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            // -------------------- Autenticación --------------------
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

            // -------------------- Middleware --------------------
            var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

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
