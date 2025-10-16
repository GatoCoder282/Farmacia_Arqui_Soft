using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Data;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repository;
using MySql.Data.MySqlClient;

namespace Farmacia_Arqui_Soft.Repositories
{
    public class LotRepository : RepositoryBase, IRepository<Lot>
    {
        private readonly DatabaseConnection _db;

        public LotRepository()
        {
            _db = DatabaseConnection.Instance;
        }

        public async Task<Lot> Create(Lot entity)
        {
            const string query = @"
                INSERT INTO lots 
                    (medicine_id, batch_number, expiration_date, quantity, unit_cost, status)
                VALUES
                    (@medicine_id, @batch_number, @expiration_date, @quantity, @unit_cost, @status)";
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@medicine_id", entity.MedicineId);
            cmd.Parameters.AddWithValue("@batch_number", entity.BatchNumber);
            cmd.Parameters.AddWithValue("@expiration_date", entity.ExpirationDate);
            cmd.Parameters.AddWithValue("@quantity", entity.Quantity);
            cmd.Parameters.AddWithValue("@unit_cost", entity.UnitCost);
            cmd.Parameters.AddWithValue("@status", entity.Status);

            await cmd.ExecuteNonQueryAsync();
            entity.Id = (int)cmd.LastInsertedId;
            return entity;
        }

        public async Task<Lot?> GetById(object id)
        {
            int key = ToIntId(id);
            const string query = @"
                SELECT id, medicine_id, batch_number, expiration_date, quantity, unit_cost, status
                FROM lots
                WHERE id=@id";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", key);

            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
            if (await reader.ReadAsync())
            {
                int oid = reader.GetOrdinal("id");
                int omid = reader.GetOrdinal("medicine_id");
                int obn = reader.GetOrdinal("batch_number");
                int oed = reader.GetOrdinal("expiration_date");
                int oq = reader.GetOrdinal("quantity");
                int ouc = reader.GetOrdinal("unit_cost");
                int ost = reader.GetOrdinal("status");

                return new Lot(
                    reader.GetInt32(oid),
                    reader.GetInt32(omid),
                    reader.GetString(obn),
                    reader.GetDateTime(oed),
                    reader.GetInt32(oq),
                    reader.GetDecimal(ouc),
                    reader.GetByte(ost)
                );
            }
            return null;
        }

        public async Task<IEnumerable<Lot>> GetAll()
        {
            var list = new List<Lot>();
            const string query = @"
                SELECT id, medicine_id, batch_number, expiration_date, quantity, unit_cost, status
                FROM lots
                WHERE status = 1";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            using var reader = await cmd.ExecuteReaderAsync();

            int oid = reader.GetOrdinal("id");
            int omid = reader.GetOrdinal("medicine_id");
            int obn = reader.GetOrdinal("batch_number");
            int oed = reader.GetOrdinal("expiration_date");
            int oq = reader.GetOrdinal("quantity");
            int ouc = reader.GetOrdinal("unit_cost");
            int ost = reader.GetOrdinal("status");

            while (await reader.ReadAsync())
            {
                list.Add(new Lot(
                    reader.GetInt32(oid),
                    reader.GetInt32(omid),
                    reader.GetString(obn),
                    reader.GetDateTime(oed),
                    reader.GetInt32(oq),
                    reader.GetDecimal(ouc),
                    reader.GetByte(ost)
                ));
            }
            return list;
        }

        public async Task Update(Lot entity)
        {
            const string query = @"
                UPDATE lots 
                SET medicine_id=@medicine_id,
                    batch_number=@batch_number,
                    expiration_date=@expiration_date,
                    quantity=@quantity,
                    unit_cost=@unit_cost,
                    status=@status
                WHERE id=@id";
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@medicine_id", entity.MedicineId);
            cmd.Parameters.AddWithValue("@batch_number", entity.BatchNumber);
            cmd.Parameters.AddWithValue("@expiration_date", entity.ExpirationDate);
            cmd.Parameters.AddWithValue("@quantity", entity.Quantity);
            cmd.Parameters.AddWithValue("@unit_cost", entity.UnitCost);
            cmd.Parameters.AddWithValue("@status", entity.Status);
            cmd.Parameters.AddWithValue("@id", entity.Id);

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task Delete(object id)
        {
            int key = ToIntId(id);
            const string query = "UPDATE lots SET status = 0 WHERE id=@id";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", key);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
