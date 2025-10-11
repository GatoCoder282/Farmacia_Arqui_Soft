// NUEVOS usings para DI de validaciones
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Validations.Users;
using Farmacia_Arqui_Soft.Validations.Clients;
using Farmacia_Arqui_Soft.Validations.Lots;

// ?? NUEVO:
using Farmacia_Arqui_Soft.Validations.Providers;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Infraestructure.Data;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

namespace Farmacia_Arqui_Soft
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            DatabaseConnection.Initialize(builder.Configuration);

            // Factory & Repositories
            builder.Services.AddSingleton<RepositoryFactory, UserRepositoryFactory>();
            builder.Services.AddScoped<IRepository<User>, UserRepository>();
            builder.Services.AddScoped<IRepository<Lot>, LotRepository>();
            builder.Services.AddSingleton<UserRepositoryFactory>();
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // ?? NUEVO: repo para Provider (si lo quieres también disponible por DI)
            builder.Services.AddScoped<IRepository<Provider>, ProviderRepository>();

            // Validators
            builder.Services.AddScoped<IValidator<User>, UserValidator>();
            builder.Services.AddScoped<IValidator<Client>, ClientValidator>();
            builder.Services.AddScoped<IValidator<Lot>, LotValidator>();

            // ?? NUEVO: validator de Provider
            builder.Services.AddScoped<IValidator<Provider>, ProviderValidator>();

            // Razor Pages
            builder.Services.AddRazorPages();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages().WithStaticAssets();

            app.Run();
        }
    }
}
