using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Validations.Providers;

namespace Farmacia_Arqui_Soft.Application.Services
{
    public class ProviderService
    {

        private readonly ProviderValidator _validator;

        public ProviderService(IRepository<Provider> repository)
        {
            _repository = repository;
            _validator = new ProviderValidator();
        }

        public async Task<IEnumerable<Provider>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<Provider?> GetByIdAsync(Provider entity)
        {
            return await _repository.GetById(entity);
        }

        public async Task CreateAsync(Provider provider)
        {
            var validation = _validator.Validate(provider);
            if (!validation.IsValid)
                throw new ArgumentException(string.Join(", ", validation.Errors.Values));

            await _repository.Create(provider);
        }

        public async Task UpdateAsync(Provider provider)
        {
            var validation = _validator.Validate(provider);
            if (!validation.IsValid)
                throw new ArgumentException(string.Join(", ", validation.Errors.Values));

            await _repository.Update(provider);
        }

        public async Task DeleteAsync(Provider provider)
        {
            await _repository.Delete(provider);
        }
    }
}
