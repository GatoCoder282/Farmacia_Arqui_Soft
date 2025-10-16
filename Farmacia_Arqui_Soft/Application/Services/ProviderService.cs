using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;
using Farmacia_Arqui_Soft.Validations.Providers;

namespace Farmacia_Arqui_Soft.Application.Services
{
    public class ProviderService
    {
        private readonly ProviderRepository _repository;
        private readonly ProviderValidator _validator;

        public ProviderService()
        {
            _repository = new ProviderRepository();
            _validator = new ProviderValidator();
        }

        public async Task<Provider> CreateAsync(Provider provider)
        {
            var validationResult = _validator.Validate(provider);
            if (!validationResult.IsValid)
                throw new ArgumentException($"Errores de validación: {string.Join(", ", validationResult.Errors)}");

            provider.CreatedAt = DateTime.UtcNow;
            provider.is_deleted = false;

            return await _repository.Create(provider);
        }

        public async Task<IEnumerable<Provider>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<Provider?> GetByIdAsync(int id)
        {
            // Corregido: creamos un Provider temporal para enviar al repositorio
            var temp = new Provider { id = id };
            return await _repository.GetById(temp);
        }

        public async Task UpdateAsync(Provider provider)
        {
            var validationResult = _validator.Validate(provider);
            if (!validationResult.IsValid)
                throw new ArgumentException($"Errores de validación: {string.Join(", ", validationResult.Errors)}");

            provider.UpdatedAt = DateTime.UtcNow;
            await _repository.Update(provider);
        }

        public async Task DeleteAsync(int id)
        {
            // Corregido: creamos un Provider temporal para enviar al repositorio
            var temp = new Provider { id = id };
            await _repository.Delete(temp);
        }
    }
}
