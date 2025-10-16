using Farmacia_Arqui_Soft.Domain.Models;

namespace Farmacia_Arqui_Soft.Application.DTOs
{
    public record UserCreateDto(
        string FirstName,
        string? SecondName,
        string LastName,
        string Mail,
        int Phone,
        string Ci,
        UserRole Role
    );

    public record UserUpdateDto(
        string? FirstName,
        string? SecondName,
        string? LastName,
        string? Mail,
        int? Phone,
        string? Ci,
        UserRole? Role,
        string? Password
    );

    // Opcional para respuestas limpias
    public record UserViewDto(
        int Id,
        string Username,
        string FirstName,
        string? SecondName,
        string LastName,
        string? Mail,
        int Phone,
        string Ci,
        UserRole Role
    );
}
