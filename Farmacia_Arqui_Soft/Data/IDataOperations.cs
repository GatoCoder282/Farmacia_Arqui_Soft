using System.Data;

namespace Farmacia_Arqui_Soft.Data
{
    public interface IDataOperations
    {
        DataTable GetAll();
        DataRow? GetById(int id);
        void Create(object entity);
        void Update(object entity);
        void Delete(int id);
    }
}
