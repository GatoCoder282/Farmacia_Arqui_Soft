using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Infraestructure.Data;
using Farmacia_Arqui_Soft.Domain.Ports;

namespace Farmacia_Arqui_Soft.Infraestructure.Persistence
{
    public class UserRepository : IRepository<User>
    {
        private readonly DatabaseConnection _db;

        public UserRepository()
        {
            _db = DatabaseConnection.Instance;
        }


        public async Task<User> Create(User entity)
        {
            string query = "INSERT INTO users (username, password, phone, ci) VALUES (@username, @password, @phone, @ci)";
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var comand = new MySqlCommand(query, connection);
            comand.Parameters.AddWithValue("@username", entity.username);
            comand.Parameters.AddWithValue("@password", entity.password);
            comand.Parameters.AddWithValue("@phone", entity.phone);
            comand.Parameters.AddWithValue("@ci", entity.ci);

            await comand.ExecuteNonQueryAsync();
            entity.id = (int)comand.LastInsertedId;
            return entity;
        }

        public async Task<User?> GetById(User entity)
        {
            string query = "SELECT * FROM users WHERE id = @id";
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var comand = new MySqlCommand(query, connection);
            comand.Parameters.AddWithValue("@id", entity.id);

            using var reader = await comand.ExecuteReaderAsync();
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
            string query = "SELECT * FROM users WHERE is_deleted = FALSE";


            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var comand = new MySqlCommand(query, connection);
            using var reader = await comand.ExecuteReaderAsync();

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
            string query = "UPDATE users SET username=@username, password=@password, phone=@phone, ci=@ci WHERE id=@id";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var comand = new MySqlCommand(query, connection);
            comand.Parameters.AddWithValue("@username", entity.username);
            comand.Parameters.AddWithValue("@password", entity.password);
            comand.Parameters.AddWithValue("@phone", entity.phone);
            comand.Parameters.AddWithValue("@ci", entity.ci);
            comand.Parameters.AddWithValue("@id", entity.id);

            await comand.ExecuteNonQueryAsync();
        }

        public async Task Delete(User entity)
        {
            string query = "UPDATE users SET is_deleted = TRUE WHERE id=@id";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var comand = new MySqlCommand(query, connection);
            comand.Parameters.AddWithValue("@id", entity.id);

            await comand.ExecuteNonQueryAsync();
        }
    }
}
