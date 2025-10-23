using Farmacia_Arqui_Soft.Modules.ClientService.Application.Dtos;
using ClientEntity = Farmacia_Arqui_Soft.Modules.ClientService.Domain.Client;
using MySqlX.XDevAPI.Common;

namespace Farmacia_Arqui_Soft.Modules.ClientService.Application
{
    public interface IClientService
    {
        Task<ClientEntity> RegisterAsync(ClientCreateDto dto, int actorId);

        Task<ClientEntity?> GetByIdAsync(int id);
        Task<IEnumerable<ClientEntity>> ListAsync();

        Task UpdateAsync(int id, ClientUpdateDto dto, int actorId);

        Task SoftDeleteAsync(int id, int actorId);
    }
}