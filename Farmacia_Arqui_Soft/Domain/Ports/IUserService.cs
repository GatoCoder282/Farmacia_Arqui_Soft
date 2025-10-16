
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Application.DTOs;

namespace Farmacia_Arqui_Soft.Domain.Ports
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserCreateDto dto, int actorId);
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> ListAsync();
        Task UpdateAsync(int id, UserUpdateDto dto, int actorId);
        Task SoftDeleteAsync(int id, int actorId);

        Task<User> AuthenticateAsync(string username, string password);

        bool CanPerformAction(User user, string action);
    }

    
}
