using Farmacia_Arqui_Soft.Data;
using Farmacia_Arqui_Soft.Factory;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repositories;

// NUEVOS usings para DI de validaciones
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Validations.Users;
using Farmacia_Arqui_Soft.Validations.Clients;
using Farmacia_Arqui_Soft.Validations.Lots;

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

            // IValidator<User> -> UserValidator
            builder.Services.AddScoped<IValidator<User>, UserValidator>();
            builder.Services.AddScoped<IValidator<Client>, ClientValidator>();
            builder.Services.AddScoped<IValidator<Lot>, LotValidator>();


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
