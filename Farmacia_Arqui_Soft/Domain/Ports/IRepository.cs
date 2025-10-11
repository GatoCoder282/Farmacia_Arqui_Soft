namespace Farmacia_Arqui_Soft.Domain.Ports
{
    public interface IRepository<T> where T : class
    {
        Task<T> Create(T entity);
        Task<T?> GetById(T entity);   
        Task<IEnumerable<T>> GetAll();
        Task Update(T entity);
        Task Delete(T entity);        
    }

}
