using Farmacia_Arqui_Soft.Data;
using Farmacia_Arqui_Soft.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Interfaces;

namespace Farmacia_Arqui_Soft.Repositories
{
    public class UserRepository : ICRUD<User>
    {
        private readonly MySqlConnection _connection;

        public UserRepository()
        {
            _connection = DatabaseConnection.Instance.Connection;
        }

        public async Task<User> Create(User entity)
        {
            string query = "INSERT INTO User (username, password, phone, ci) VALUES (@username, @password, @phone, @ci)";
            using var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@username", entity.username);
            cmd.Parameters.AddWithValue("@password", entity.password);
            cmd.Parameters.AddWithValue("@phone", entity.phone);
            cmd.Parameters.AddWithValue("@ci", entity.ci);

            await cmd.ExecuteNonQueryAsync();
            entity.id = (int)cmd.LastInsertedId;
            return entity;
        }

        public async Task<User> GetById(int id)
        {
            string query = "SELECT * FROM User WHERE id = @id";
            using var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User(
                    reader.GetInt32("id"),
                    reader.GetString("username"),
                    reader.GetString("password"),
                    reader.GetInt32("phone"),
                    reader.GetString("ci")
                );
            }
            return null;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var lista = new List<User>();
            string query = "SELECT * FROM User";
            using var cmd = new MySqlCommand(query, _connection);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new User(
                    reader.GetInt32("id"),
                    reader.GetString("username"),
                    reader.GetString("password"),
                    reader.GetInt32("phone"),
                    reader.GetString("ci")
                ));
            }
            return lista;
        }

        public async Task Update(User entity)
        {
            string query = "UPDATE User SET username=@username, password=@password, phone=@phone, ci=@ci WHERE id=@id";
            using var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@username", entity.username);
            cmd.Parameters.AddWithValue("@password", entity.password);
            cmd.Parameters.AddWithValue("@phone", entity.phone);
            cmd.Parameters.AddWithValue("@ci", entity.ci);
            cmd.Parameters.AddWithValue("@id", entity.id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(int id)
        {
            string query = "DELETE FROM User WHERE id=@id";
            using var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
