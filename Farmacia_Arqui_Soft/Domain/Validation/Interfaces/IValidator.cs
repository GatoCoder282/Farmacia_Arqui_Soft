using FluentResults;

namespace Farmacia_Arqui_Soft.Validations.Interfaces
{
    public interface IValidator<T>
    {
        Result Validate(T entity);
    }
}