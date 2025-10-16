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
                    (first_name, last_name, nit, address, email, phone, status, is_deleted, created_at)
                VALUES
                    (@first_name, @last_name, @nit, @address, @email, @phone, @status, @is_deleted, @created_at);";

            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@first_name", entity.firstName);
            cmd.Parameters.AddWithValue("@last_name", entity.lastName);
            cmd.Parameters.AddWithValue("@nit", (object?)entity.nit ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object?)entity.address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)entity.email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", (object?)entity.phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@status", entity.status);        // 1 activo, 0 inactivo
            cmd.Parameters.AddWithValue("@is_deleted", entity.is_deleted);    // false por defecto
            cmd.Parameters.AddWithValue("@created_at", entity.CreatedAt == default ? DateTime.UtcNow : entity.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
            entity.id = (int)cmd.LastInsertedId;
            return entity;
        }

        public async Task<Provider?> GetById(Provider entity)
        {
            const string sql = @"SELECT * FROM providers WHERE id=@id;";
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", entity.id);

            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
            if (await reader.ReadAsync())
            {
                int o_nit = reader.GetOrdinal("nit");
                int o_addr = reader.GetOrdinal("address");
                int o_mail = reader.GetOrdinal("email");
                int o_ph = reader.GetOrdinal("phone");
                int o_ca = reader.GetOrdinal("created_at");
                int o_ua = reader.GetOrdinal("updated_at");

                return new Provider
                {
                    id = reader.GetInt32("id"),
                    firstName = reader.GetString("first_name"),
                    lastName = reader.GetString("last_name"),
                    nit = reader.IsDBNull(o_nit) ? null : reader.GetString(o_nit),
                    address = reader.IsDBNull(o_addr) ? null : reader.GetString(o_addr),
                    email = reader.IsDBNull(o_mail) ? null : reader.GetString(o_mail),
                    phone = reader.IsDBNull(o_ph) ? null : reader.GetString(o_ph),
                    status = reader.GetByte("status"),
                    is_deleted = reader.GetBoolean("is_deleted"),
                    // CreatedAt es no-nullable: si viene NULL en DB, usa DateTime.MinValue
                    CreatedAt = reader.IsDBNull(o_ca) ? DateTime.MinValue : reader.GetDateTime(o_ca),
                    UpdatedAt = reader.IsDBNull(o_ua) ? (DateTime?)null : reader.GetDateTime(o_ua)
                };
            }
            return null;
        }

        public async Task<IEnumerable<Provider>> GetAll()
        {
            var list = new List<Provider>();
            const string sql = @"
                SELECT *
                FROM providers
                WHERE is_deleted = FALSE AND status = 1
                ORDER BY first_name ASC;";

            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            int o_nit = reader.GetOrdinal("nit");
            int o_addr = reader.GetOrdinal("address");
            int o_mail = reader.GetOrdinal("email");
            int o_ph = reader.GetOrdinal("phone");
            int o_ca = reader.GetOrdinal("created_at");
            int o_ua = reader.GetOrdinal("updated_at");

            while (await reader.ReadAsync())
            {
                list.Add(new Provider
                {
                    id = reader.GetInt32("id"),
                    firstName = reader.GetString("first_name"),
                    lastName = reader.GetString("last_name"),
                    nit = reader.IsDBNull(o_nit) ? null : reader.GetString(o_nit),
                    address = reader.IsDBNull(o_addr) ? null : reader.GetString(o_addr),
                    email = reader.IsDBNull(o_mail) ? null : reader.GetString(o_mail),
                    phone = reader.IsDBNull(o_ph) ? null : reader.GetString(o_ph),
                    status = reader.GetByte("status"),
                    is_deleted = reader.GetBoolean("is_deleted"),
                    CreatedAt = reader.IsDBNull(o_ca) ? DateTime.MinValue : reader.GetDateTime(o_ca),
                    UpdatedAt = reader.IsDBNull(o_ua) ? (DateTime?)null : reader.GetDateTime(o_ua)
                });
            }
            return list;
        }

        public async Task Update(Provider entity)
        {
            const string sql = @"
                UPDATE providers
                SET first_name=@first_name,
                    last_name=@last_name,
                    nit=@nit,
                    address=@address,
                    email=@email,
                    phone=@phone,
                    status=@status,
                    updated_at=@updated_at
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
            cmd.Parameters.AddWithValue("@updated_at", entity.UpdatedAt ?? DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@id", entity.id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(Provider entity)
        {
            // Soft delete: marca eliminado y fecha de actualización
            const string sql = @"
                UPDATE providers
                SET is_deleted = TRUE,
                    updated_at = @updated_at
                WHERE id=@id;";

            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", entity.id);
            cmd.Parameters.AddWithValue("@updated_at", entity.UpdatedAt ?? DateTime.UtcNow);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
