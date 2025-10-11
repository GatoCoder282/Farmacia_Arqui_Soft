using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Data;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Repository;
using MySql.Data.MySqlClient;

namespace Farmacia_Arqui_Soft.Repositories
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
                    (first_name, last_name, nit, address, email, phone, status)
                VALUES
                    (@first_name, @last_name, @nit, @address, @email, @phone, @status);";

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

            await cmd.ExecuteNonQueryAsync();
            entity.id = Convert.ToInt32(cmd.LastInsertedId); // <-- int seguro
            return entity;
        }

        public async Task<Provider?> GetById(Provider entity)
        {
            const string sql = @"SELECT id, first_name, last_name, nit, address, email, phone, status
                                 FROM providers
                                 WHERE id = @id;";

            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", entity.id);

            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
            if (await reader.ReadAsync())
            {
                // Usamos ordinales + tipos ADO.NET
                int oId = reader.GetOrdinal("id");
                int ofn = reader.GetOrdinal("first_name");
                int oln = reader.GetOrdinal("last_name");
                int onit = reader.GetOrdinal("nit");
                int oaddr = reader.GetOrdinal("address");
                int oem = reader.GetOrdinal("email");
                int oph = reader.GetOrdinal("phone");
                int ost = reader.GetOrdinal("status");

                return new Provider
                {
                    id = reader.GetInt32(oId),
                    firstName = reader.GetString(ofn),
                    lastName = reader.GetString(oln),
                    nit = reader.IsDBNull(onit) ? null : reader.GetString(onit),
                    address = reader.IsDBNull(oaddr) ? null : reader.GetString(oaddr),
                    email = reader.IsDBNull(oem) ? null : reader.GetString(oem),
                    phone = reader.IsDBNull(oph) ? null : reader.GetString(oph),
                    status = reader.GetByte(ost)
                };
            }
            return null;
        }

        public async Task<IEnumerable<Provider>> GetAll()
        {
            var list = new List<Provider>();
            string sql = "SELECT * FROM providers WHERE is_deleted = FALSE";
            

            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            // Pre-resolvemos ordinales una sola vez
            int oId = reader.GetOrdinal("id");
            int ofn = reader.GetOrdinal("first_name");
            int oln = reader.GetOrdinal("last_name");
            int onit = reader.GetOrdinal("nit");
            int oaddr = reader.GetOrdinal("address");
            int oem = reader.GetOrdinal("email");
            int oph = reader.GetOrdinal("phone");
            int ost = reader.GetOrdinal("status");

            while (await reader.ReadAsync())
            {
                list.Add(new Provider
                {
                    id = reader.GetInt32(oId),
                    firstName = reader.GetString(ofn),
                    lastName = reader.GetString(oln),
                    nit = reader.IsDBNull(onit) ? null : reader.GetString(onit),
                    address = reader.IsDBNull(oaddr) ? null : reader.GetString(oaddr),
                    email = reader.IsDBNull(oem) ? null : reader.GetString(oem),
                    phone = reader.IsDBNull(oph) ? null : reader.GetString(oph),
                    status = reader.GetByte(ost)
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
                    status=@status
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
            cmd.Parameters.AddWithValue("@id", entity.id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(Provider entity)
        {
            string sql = "UPDATE providers SET is_deleted = TRUE WHERE id=@id";

            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", entity.id);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
