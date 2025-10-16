using Farmacia_Arqui_Soft.Application.DTOs;
using Farmacia_Arqui_Soft.Domain.Models;
using MySqlX.XDevAPI.Common;

namespace Farmacia_Arqui_Soft.Domain.Ports
{
    public interface IClientService
    {
        Task<Client> RegisterAsync(ClientCreateDto dto, int actorId);

        Task<Client?> GetByIdAsync(int id);
        Task<IEnumerable<Client>> ListAsync();

        Task UpdateAsync(int id, ClientUpdateDto dto, int actorId);

        Task SoftDeleteAsync(int id, int actorId);
    }
}
