using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Data;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repository;
using MySql.Data.MySqlClient;

namespace Farmacia_Arqui_Soft.Repositories
{
    public class UserRepository : RepositoryBase, IRepository<User>
    {
        private readonly DatabaseConnection _db;

        public UserRepository()
        {
            _db = DatabaseConnection.Instance;
        }

        public async Task<User> Create(User entity)
        {
            const string query = @"
                INSERT INTO users (username, password, phone, ci, status)
                VALUES (@username, @password, @phone, @ci, @status)";
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@username", entity.username);
            cmd.Parameters.AddWithValue("@password", entity.password);
            cmd.Parameters.AddWithValue("@phone", entity.phone);
            cmd.Parameters.AddWithValue("@ci", entity.ci);
            cmd.Parameters.AddWithValue("@status", entity.status);

            await cmd.ExecuteNonQueryAsync();
            entity.id = Convert.ToInt32(cmd.LastInsertedId);
            return entity;
        }

        public async Task<User?> GetById(object id)
        {
            int key = ToIntId(id);
            const string query = @"
                SELECT id, username, password, phone, ci, status
                FROM users
                WHERE id = @id";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", key);

            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
            if (await reader.ReadAsync())
            {
                int oid = reader.GetOrdinal("id");
                int ou = reader.GetOrdinal("username");
                int op = reader.GetOrdinal("password");
                int oph = reader.GetOrdinal("phone");
                int oci = reader.GetOrdinal("ci");
                int ost = reader.GetOrdinal("status");

                return new User(
                    reader.GetInt32(oid),
                    reader.GetString(ou),
                    reader.GetString(op),
                    reader.GetInt32(oph),
                    reader.GetString(oci),
                    reader.GetByte(ost)
                );
            }
            return null;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var list = new List<User>();
            const string query = @"
                SELECT id, username, password, phone, ci, status
                FROM users
                WHERE status = 1";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            using var reader = await cmd.ExecuteReaderAsync();

            int oid = reader.GetOrdinal("id");
            int ou = reader.GetOrdinal("username");
            int op = reader.GetOrdinal("password");
            int oph = reader.GetOrdinal("phone");
            int oci = reader.GetOrdinal("ci");
            int ost = reader.GetOrdinal("status");

            while (await reader.ReadAsync())
            {
                list.Add(new User(
                    reader.GetInt32(oid),
                    reader.GetString(ou),
                    reader.GetString(op),
                    reader.GetInt32(oph),
                    reader.GetString(oci),
                    reader.GetByte(ost)
                ));
            }
            return list;
        }

        public async Task Update(User entity)
        {
            const string query = @"
                UPDATE users
                SET username=@username,
                    password=@password,
                    phone=@phone,
                    ci=@ci,
                    status=@status
                WHERE id=@id";
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@username", entity.username);
            cmd.Parameters.AddWithValue("@password", entity.password);
            cmd.Parameters.AddWithValue("@phone", entity.phone);
            cmd.Parameters.AddWithValue("@ci", entity.ci);
            cmd.Parameters.AddWithValue("@status", entity.status);
            cmd.Parameters.AddWithValue("@id", entity.id);

            await cmd.ExecuteNonQueryAsync();
        }

  
        public async Task Delete(object id)
        {
            int key = ToIntId(id);
            const string query = "UPDATE users SET status = 0 WHERE id=@id";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", key);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
