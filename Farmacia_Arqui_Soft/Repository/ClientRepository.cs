using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Data;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repository;
using MySql.Data.MySqlClient;

namespace Farmacia_Arqui_Soft.Repository
{
    public class ClientRepository : RepositoryBase, IRepository<Client>
    {
        private readonly DatabaseConnection _db;

        public ClientRepository()
        {
            _db = DatabaseConnection.Instance;
        }

        public async Task<Client> Create(Client entity)
        {
            const string query = @"
                INSERT INTO clients (first_name, last_name, nit, email)
                VALUES (@first_name, @last_name, @nit, @email);";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@first_name", entity.first_name);
            cmd.Parameters.AddWithValue("@last_name", entity.last_name);
            cmd.Parameters.AddWithValue("@nit", entity.nit);
            cmd.Parameters.AddWithValue("@email", entity.email);

            await cmd.ExecuteNonQueryAsync();
            entity.id = (int)cmd.LastInsertedId;
            return entity;
        }

        public async Task<Client?> GetById(object id)
        {
            int key = ToIntId(id);
            const string query = "SELECT id, first_name, last_name, nit, email FROM clients WHERE id = @id;";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", key);

            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
            if (await reader.ReadAsync())
            {
                int oid = reader.GetOrdinal("id");
                int ofn = reader.GetOrdinal("first_name");
                int oln = reader.GetOrdinal("last_name");
                int on = reader.GetOrdinal("nit");
                int oe = reader.GetOrdinal("email");

                return new Client(
                    reader.GetInt32(oid),
                    reader.GetString(ofn),
                    reader.GetString(oln),
                    reader.GetString(on),
                    reader.GetString(oe)
                );
            }
            return null;
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            var list = new List<Client>();
            const string query = "SELECT id, first_name, last_name, nit, email FROM clients;";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            using var reader = await cmd.ExecuteReaderAsync();

            int oid = reader.GetOrdinal("id");
            int ofn = reader.GetOrdinal("first_name");
            int oln = reader.GetOrdinal("last_name");
            int on = reader.GetOrdinal("nit");
            int oe = reader.GetOrdinal("email");

            while (await reader.ReadAsync())
            {
                list.Add(new Client(
                    reader.GetInt32(oid),
                    reader.GetString(ofn),
                    reader.GetString(oln),
                    reader.GetString(on),
                    reader.GetString(oe)
                ));
            }
            return list;
        }

        public async Task Update(Client entity)
        {
            const string query = @"
                UPDATE clients 
                SET first_name = @first_name,
                    last_name  = @last_name,
                    nit        = @nit,
                    email      = @email
                WHERE id = @id;";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@first_name", entity.first_name);
            cmd.Parameters.AddWithValue("@last_name", entity.last_name);
            cmd.Parameters.AddWithValue("@nit", entity.nit);
            cmd.Parameters.AddWithValue("@email", entity.email);
            cmd.Parameters.AddWithValue("@id", entity.id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(object id)
        {
            int key = ToIntId(id);
            const string query = "DELETE FROM clients WHERE id = @id;";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", key);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
