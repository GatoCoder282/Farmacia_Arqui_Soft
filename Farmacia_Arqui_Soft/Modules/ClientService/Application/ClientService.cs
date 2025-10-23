using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Domain.Ports.UserPorts;
using Farmacia_Arqui_Soft.Modules.ClientService.Application.Dtos;
// NO USAR: using Farmacia_Arqui_Soft.Modules.ClientService.Domain; <-- Causa el error

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// Alias para el tipo Client:
using ClientEntity = Farmacia_Arqui_Soft.Modules.ClientService.Domain.Client;


namespace Farmacia_Arqui_Soft.Modules.ClientService.Application
{
    public class ClientService : IClientService
    {
        private readonly IRepository<ClientEntity> _clientRepository; // Usa ClientEntity
        private readonly IUserService _userService;

        public ClientService(IRepository<ClientEntity> clientRepository, IUserService userService) // Usa ClientEntity
        {
            _clientRepository = clientRepository;
            _userService = userService;
        }

        public async Task<ClientEntity> RegisterAsync(ClientCreateDto dto, int actorId) // Usa ClientEntity
        {
            var actor = await _userService.GetByIdAsync(actorId);
            if (actor == null || !_userService.CanPerformAction(actor, "create_client"))
            {
                throw new InvalidOperationException("El usuario no tiene permisos para registrar clientes.");
            }

            var newClient = new ClientEntity // Usa ClientEntity
            {
                first_name = dto.FirstName,
                last_name = dto.LastName,
                nit = dto.nit,
                email = dto.email,
                is_deleted = false
            };

            var createdClient = await _clientRepository.Create(newClient);

            return createdClient;
        }

        public async Task<ClientEntity?> GetByIdAsync(int id) // Usa ClientEntity
        {
            var clientReference = new ClientEntity { id = id }; // Usa ClientEntity
            return await _clientRepository.GetById(clientReference);
        }

        public async Task<IEnumerable<ClientEntity>> ListAsync() // Usa ClientEntity
        {
            return await _clientRepository.GetAll();
        }

        public async Task UpdateAsync(int id, ClientUpdateDto dto, int actorId)
        {
            var actor = await _userService.GetByIdAsync(actorId);
            if (actor == null || !_userService.CanPerformAction(actor, "update_client"))
            {
                throw new InvalidOperationException("El usuario no tiene permisos para actualizar clientes.");
            }

            var clientReference = new ClientEntity { id = id }; // Usa ClientEntity
            var existingClient = await _clientRepository.GetById(clientReference);

            if (existingClient == null)
            {
                throw new ArgumentException($"Cliente con ID {id} no encontrado.");
            }

            existingClient.first_name = dto.FirstName;
            existingClient.last_name = dto.LastName;
            existingClient.nit = dto.nit;
            existingClient.email = dto.email;

            await _clientRepository.Update(existingClient);
        }

        public async Task SoftDeleteAsync(int id, int actorId)
        {
            var actor = await _userService.GetByIdAsync(actorId);
            if (actor == null || !_userService.CanPerformAction(actor, "delete_client"))
            {
                throw new InvalidOperationException("El usuario no tiene permisos para eliminar clientes.");
            }

            var clientReference = new ClientEntity { id = id }; // Usa ClientEntity
            var existingClient = await _clientRepository.GetById(clientReference);

            if (existingClient == null)
            {
                throw new ArgumentException($"Cliente con ID {id} no encontrado.");
            }

            await _clientRepository.Delete(existingClient);
        }
    }
}