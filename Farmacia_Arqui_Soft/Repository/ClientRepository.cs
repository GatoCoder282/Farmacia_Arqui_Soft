using Farmacia_Arqui_Soft.Data;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Farmacia_Arqui_Soft.Repositories
{
    public class ClientRepository : IRepository<Client>
    {
        private readonly DatabaseConnection _db;

        public ClientRepository()
        {
            _db = DatabaseConnection.Instance;
        }

        public async Task<Client> Create(Client entity)
        {
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            const string query = @"
                INSERT INTO Client (first_name, last_name, nit, email)
                VALUES (@first_name, @last_name, @nit, @email);
            ";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@first_name", entity.first_name);
            cmd.Parameters.AddWithValue("@last_name", entity.last_name);
            cmd.Parameters.AddWithValue("@nit", entity.nit);
            cmd.Parameters.AddWithValue("@email", entity.email);

            await cmd.ExecuteNonQueryAsync();
            entity.id = (int)cmd.LastInsertedId;
            return entity;
        }

        public async Task<Client> GetById(int id)
        {
            using var connection = _db.GetConnection();
            await connection.OpenAsync();
            const string query = "SELECT id, first_name, last_name, nit, email FROM Client WHERE id = @id;";
            using var cmd = new MySqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Client(
                    reader.GetInt32("id"),
                    reader.GetString("first_name"),
                    reader.GetString("last_name"),
                    reader.GetString("nit"),
                    reader.GetString("email")
                );
            }
            return null;
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            var list = new List<Client>();

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            const string query = "SELECT id, first_name, last_name, nit, email FROM Client;";

            using var cmd = new MySqlCommand(query, connection);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Client(
                    reader.GetInt32("id"),
                    reader.GetString("first_name"),
                    reader.GetString("last_name"),
                    reader.GetString("nit"),
                    reader.GetString("email")
                ));
            }
            
            return list;
        }

        public async Task Update(Client entity)
        {
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            const string query = @"
                UPDATE Client 
                SET first_name = @first_name,
                    last_name  = @last_name,
                    nit        = @nit,
                    email      = @email
                WHERE id = @id;
            ";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@first_name", entity.first_name);
            cmd.Parameters.AddWithValue("@last_name", entity.last_name);
            cmd.Parameters.AddWithValue("@nit", entity.nit);
            cmd.Parameters.AddWithValue("@email", entity.email);
            cmd.Parameters.AddWithValue("@id", entity.id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(int id)
        {
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            const string query = "DELETE FROM Client WHERE id = @id;";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
