using System.Collections.Generic;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;
using Farmacia_Arqui_Soft.Validations.Interfaces;

namespace Farmacia_Arqui_Soft.Application.Services
{
    public class LotService
    {
        private readonly IRepository<Lot> _repository;
        private readonly IValidator<Lot> _validator;

        public LotService(IValidator<Lot> validator)
        {
            _validator = validator;
            var factory = new LotRepositoryFactory();
            _repository = factory.CreateRepository<Lot>();
        }
        public async Task<IEnumerable<Lot>> GetAllAsync() =>
            await _repository.GetAll();

        public async Task<Lot?> GetByIdAsync(int id) =>
            await _repository.GetById(new Lot { Id = id });
        public async Task<(bool Success, Dictionary<string, string>? Errors)> CreateAsync(Lot lot)
        {
            var validation = _validator.Validate(lot);
            if (!validation.IsValid)
                return (false, validation.Errors);

            await _repository.Create(lot);
            return (true, null);
        }
        public async Task<(bool Success, Dictionary<string, string>? Errors)> UpdateAsync(Lot lot)
        {
            var validation = _validator.Validate(lot);
            if (!validation.IsValid)
                return (false, validation.Errors);

            await _repository.Update(lot);
            return (true, null);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetById(new Lot { Id = id });
            if (existing == null)
                return false;

            await _repository.Delete(existing);
            return true;
        }
    }
}
