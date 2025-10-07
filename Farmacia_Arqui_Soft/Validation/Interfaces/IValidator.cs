namespace Farmacia_Arqui_Soft.Validations.Interfaces
{
    public interface IValidator<T>
    {
        ValidationResult Validate(T entity);
    }
}
