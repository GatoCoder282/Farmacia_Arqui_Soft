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
            string query = @"INSERT INTO users 
                            (username, password, mail, phone, ci, role, created_at, created_by, is_deleted) 
                            VALUES (@username, @password, @mail, @phone, @ci, @role, @created_at, @created_by, @is_deleted)";
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var comand = new MySqlCommand(query, connection);
            comand.Parameters.AddWithValue("@username", entity.username);
            comand.Parameters.AddWithValue("@password", entity.password);
            comand.Parameters.AddWithValue("@mail", entity.mail);
            comand.Parameters.AddWithValue("@phone", entity.phone);
            comand.Parameters.AddWithValue("@ci", entity.ci);
            comand.Parameters.AddWithValue("@role", entity.role.ToString());
            comand.Parameters.AddWithValue("@created_at", entity.created_at);
            comand.Parameters.AddWithValue("@created_by", entity.created_by);
            comand.Parameters.AddWithValue("@is_deleted", entity.is_deleted);


            await comand.ExecuteNonQueryAsync();
            entity.id = (int)comand.LastInsertedId;
            return entity;
        }

        public async Task<User?> GetById(User entity)
        {
            string query = "SELECT * FROM users WHERE id = @id AND is_deleted = FALSE";
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
                    reader.GetString("mail"),
                    reader.GetInt32("phone"),
                    reader.GetString("ci"),
                    Enum.Parse<UserRole>(reader.GetString("role")),
                    reader.GetBoolean("is_deleted"),
                    reader.GetInt32("created_by"),
                    reader.GetInt32("updated_by"),
                    reader.GetDateTime("created_at"),
                    reader.GetDateTime("updated_at")

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
                    reader.GetString("mail"),
                    reader.GetInt32("phone"),
                    reader.GetString("ci"),
                    Enum.Parse<UserRole>(reader.GetString("role")),
                    reader.GetBoolean("is_deleted"),
                    reader.GetInt32("created_by"),
                    reader.GetInt32("updated_by"),
                    reader.GetDateTime("created_at"),
                    reader.GetDateTime("updated_at")
                ));
            }
            return lista;
        }

        public async Task Update(User entity)
        {
            string query = @"UPDATE users 
                             SET username=@username, password=@password, mail=@mail, phone=@phone, ci=@ci, 
                                 role=@role, updated_at=@updated_at, updated_by=@updated_by 
                             WHERE id=@id";
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var comand = new MySqlCommand(query, connection);
            comand.Parameters.AddWithValue("@username", entity.username);
            comand.Parameters.AddWithValue("@password", entity.password);
            comand.Parameters.AddWithValue("@mail", entity.mail);
            comand.Parameters.AddWithValue("@phone", entity.phone);
            comand.Parameters.AddWithValue("@ci", entity.ci);
            comand.Parameters.AddWithValue("@role", entity.role.ToString());
            comand.Parameters.AddWithValue("@updated_at", DateTime.Now);
            comand.Parameters.AddWithValue("@updated_by", entity.updated_by);
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
