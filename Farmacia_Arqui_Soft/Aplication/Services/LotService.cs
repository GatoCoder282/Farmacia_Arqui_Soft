using System.Collections.Generic;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Factory;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Repository;

namespace Farmacia_Arqui_Soft.Application.Services
{
    public class LotService
    {
        private readonly IRepository<Lot> _lotRepository;
        private readonly IValidator<Lot> _validator;

        public LotService(IValidator<Lot> validator)
        {
            _validator = validator;
            var factory = new LotRepositoryFactory();
            _lotRepository = factory.CreateRepository<Lot>();
        }
        public async Task<IEnumerable<Lot>> GetAllAsync()
        {
            return await _lotRepository.GetAll();
        }
        public async Task<Lot?> GetByIdAsync(int id)
        {
            var temp = new Lot { Id = id };
            return await _lotRepository.GetById(temp);
        }

        public async Task<(bool Success, Dictionary<string, string>? Errors)> CreateAsync(Lot lot)
        {
            var result = _validator.Validate(lot);
            if (!result.IsValid)
                return (false, result.Errors);

            await _lotRepository.Create(lot);
            return (true, null);
        }

        public async Task<(bool Success, Dictionary<string, string>? Errors)> UpdateAsync(Lot lot)
        {
            var result = _validator.Validate(lot);
            if (!result.IsValid)
                return (false, result.Errors);

            await _lotRepository.Update(lot);
            return (true, null);
        }

        public async Task DeleteAsync(int id)
        {
            var temp = new Lot { Id = id };
            await _lotRepository.Delete(temp);
        }
    }
}
