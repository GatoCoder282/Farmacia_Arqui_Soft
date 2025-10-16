using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Validations.Providers;   
using Farmacia_Arqui_Soft.Validations.Interfaces;  
using Farmacia_Arqui_Soft.Infraestructure.Persistence;
       

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
            if (!validationResult.IsSuccess)
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
            return await _repository.GetById(new Provider { id = id });
        }

        public async Task UpdateAsync(Provider provider)
        {
            var validationResult = _validator.Validate(provider);
            if (!validationResult.IsSuccess)
                throw new ArgumentException($"Errores de validación: {string.Join(", ", validationResult.Errors)}");

            provider.UpdatedAt = DateTime.UtcNow;
            await _repository.Update(provider);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(new Provider { id = id });
        }
    }
}
