namespace Farmacia_Arqui_Soft.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> Create(T entity);
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task Update(T entity);
        Task Delete(int id);
    }
}
