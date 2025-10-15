
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Application.DTOS;

namespace Farmacia_Arqui_Soft.Domain.Ports.UserPorts
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserCreateDto dto, int actorId);
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> ListAsync();
        Task UpdateAsync(int id, UserUpdateDto dto, int actorId);
        Task SoftDeleteAsync(int id, int actorId);

        // Login por username + password
        Task<User> AuthenticateAsync(string username, string password);

        // Autorización simple por rol/acción
        bool CanPerformAction(User user, string action);
    }

    
}
