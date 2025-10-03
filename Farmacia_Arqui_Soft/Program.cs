using Farmacia_Arqui_Soft.Factory;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repositories;

namespace Farmacia_Arqui_Soft
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<RepositoryFactory, UserRepositoryFactory>();

            builder.Services.AddScoped<IRepository<User>, UserRepository>();

            builder.Services.AddSingleton<UserRepositoryFactory>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
