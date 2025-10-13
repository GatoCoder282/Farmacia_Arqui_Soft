using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

namespace Farmacia_Arqui_Soft.Domain.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;

        public UserService()
        {
            _repository = new UserRepository();
        }

        // ---------------------------------------------
        // Autenticación básica por rol
        // ---------------------------------------------
        public bool CanPerformAction(User user, string action)
        {
            if (user.role == UserRole.Administrador)
                return true;

            // Ejemplo: Cajero solo puede "vender" o "ver datos"
            var allowedActions = new List<string> { "Vender", "VerMisDatos" };
            return allowedActions.Contains(action);
        }

        // ---------------------------------------------
        // Crear usuario (con auditoría y rol)
        // ---------------------------------------------
        public async Task<User> CreateUserAsync(string password, string mail, int phone, string ci,
                                                UserRole role, int createdBy)
        {
            var newUser = new User
            {
                username = GenerateUsername(ci),
                password = password,
                mail = mail,
                phone = phone,
                ci = ci,
                role = role,
                created_by = createdBy,
                updated_by = createdBy
            };

            return await _repository.Create(newUser);
        }

        // ---------------------------------------------
        // Generar username automático a partir del CI
        // ---------------------------------------------
        public string GenerateUsername(string ci)
        {
            // Ejemplo simple: "user_" + last 4 dígitos del CI + timestamp
            return $"user_{ci.Substring(Math.Max(0, ci.Length - 4))}_{DateTime.Now:HHmmss}";
        }

        // ---------------------------------------------
        // Otros métodos útiles
        // ---------------------------------------------
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _repository.GetById(new User { id = id });
        }
    }
}
