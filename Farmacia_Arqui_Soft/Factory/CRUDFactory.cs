using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repositories;
using Farmacia_Arqui_Soft.Interfaces;

namespace Farmacia_Arqui_Soft.Factory
{
    public class CRUDFactory
    {
        public ICRUD<T> CreateCrud<T>() where T : class
        {
            if (typeof(T) == typeof(User))
                return (ICRUD<T>)new UserRepository();

            throw new NotImplementedException($"No existe implementación CRUD para {typeof(T).Name}");
        }
    }
}
