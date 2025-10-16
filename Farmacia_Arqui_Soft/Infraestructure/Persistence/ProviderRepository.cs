using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Infraestructure.Data;
using MySql.Data.MySqlClient;

namespace Farmacia_Arqui_Soft.Infraestructure.Persistence
{
    public class ProviderRepository : IRepository<Provider>
    {
        private readonly DatabaseConnection _db;

        public ProviderRepository()
        {
            _db = DatabaseConnection.Instance;
        }

        public async Task<Provider> Create(Provider entity)
        {
            const string sql = @"
                INSERT INTO providers
                    (first_name, last_name, nit, address, email, phone, status, is_deleted, CreatedAt)
                VALUES
                    (@first_name, @last_name, @nit, @address, @email, @phone, @status, @is_deleted, @CreatedAt);";

            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@first_name", entity.firstName);
            cmd.Parameters.AddWithValue("@last_name", entity.lastName);
            cmd.Parameters.AddWithValue("@nit", (object?)entity.nit ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object?)entity.address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)entity.email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", (object?)entity.phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@status", entity.status);
            cmd.Parameters.AddWithValue("@is_deleted", entity.is_deleted);
            cmd.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
            entity.id = Convert.ToInt32(cmd.LastInsertedId);
            return entity;
        }

        public async Task<Provider?> GetById(Provider entity)
        {
            const string sql = @"SELECT * FROM providers WHERE id=@id";
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", entity.id);
            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
            if (await reader.ReadAsync())
            {
                return new Provider
                {
                    id = reader.GetInt32("id"),
                    firstName = reader.GetString("first_name"),
                    lastName = reader.GetString("last_name"),
                    nit = reader.IsDBNull(reader.GetOrdinal("nit")) ? null : reader.GetString("nit"),
                    address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString("address"),
                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                    phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString("phone"),
                    status = reader.GetByte("status"),
                    is_deleted = reader.GetBoolean("is_deleted")
                };
            }
            return null;
        }

        public async Task<IEnumerable<Provider>> GetAll()
        {
            var list = new List<Provider>();
            const string sql = "SELECT * FROM providers WHERE is_deleted = FALSE";
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Provider
                {
                    id = reader.GetInt32("id"),
                    firstName = reader.GetString("first_name"),
                    lastName = reader.GetString("last_name"),
                    nit = reader.IsDBNull(reader.GetOrdinal("nit")) ? null : reader.GetString("nit"),
                    address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString("address"),
                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                    phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString("phone"),
                    status = reader.GetByte("status"),
                    is_deleted = reader.GetBoolean("is_deleted")
                });
            }
            return list;
        }

        public async Task Update(Provider entity)
        {
            const string sql = @"
                UPDATE providers
                SET first_name=@first_name, last_name=@last_name, nit=@nit,
                    address=@address, email=@email, phone=@phone, status=@status, UpdatedAt=@UpdatedAt
                WHERE id=@id;";
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@first_name", entity.firstName);
            cmd.Parameters.AddWithValue("@last_name", entity.lastName);
            cmd.Parameters.AddWithValue("@nit", (object?)entity.nit ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object?)entity.address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)entity.email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", (object?)entity.phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@status", entity.status);
            cmd.Parameters.AddWithValue("@UpdatedAt", entity.UpdatedAt);
            cmd.Parameters.AddWithValue("@id", entity.id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(Provider entity)
        {
            const string sql = "UPDATE providers SET is_deleted=TRUE WHERE id=@id";
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", entity.id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
