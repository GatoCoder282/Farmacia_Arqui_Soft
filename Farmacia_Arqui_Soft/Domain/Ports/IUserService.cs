
using Farmacia_Arqui_Soft.Domain.Models;

namespace Farmacia_Arqui_Soft.Domain.Ports
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

    // Alta: SIN password (se genera) y SIN username (se genera)
    public record UserCreateDto(
        string FirstName,
        string? SecondName,
        string LastName,
        string Mail,
        int Phone,
        string Ci,
        UserRole Role
    );

    // Update: todo opcional, y NO se permite editar username
    public record UserUpdateDto(
        string? FirstName,
        string? SecondName,
        string? LastName,
        string? Mail,
        int? Phone,
        string? Ci,
        UserRole? Role,
        string? Password // si quisieras resetear manualmente
    );
}
