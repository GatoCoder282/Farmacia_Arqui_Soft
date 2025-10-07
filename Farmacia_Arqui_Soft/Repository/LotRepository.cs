using Farmacia_Arqui_Soft.Data;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Farmacia_Arqui_Soft.Repositories
{
    public class LotRepository : IRepository<Lot>
    {
        private readonly DatabaseConnection _db;

        public LotRepository()
        {
            _db = DatabaseConnection.Instance;
        }

        public async Task<Lot> Create(Lot entity)
        {
            string query = @"INSERT INTO lots 
                            (medicine_id, batch_number, expiration_date, quantity, unit_cost)
                            VALUES (@medicine_id, @batch_number, @expiration_date, @quantity, @unit_cost)";
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@medicine_id", entity.MedicineId);
            cmd.Parameters.AddWithValue("@batch_number", entity.BatchNumber);
            cmd.Parameters.AddWithValue("@expiration_date", entity.ExpirationDate);
            cmd.Parameters.AddWithValue("@quantity", entity.Quantity);
            cmd.Parameters.AddWithValue("@unit_cost", entity.UnitCost);

            await cmd.ExecuteNonQueryAsync();
            entity.Id = (int)cmd.LastInsertedId;
            return entity;
        }

        public async Task<Lot?> GetById(int id)
        {
            string query = "SELECT * FROM lots WHERE id=@id";
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Lot(
                    reader.GetInt32("id"),
                    reader.GetInt32("medicine_id"),
                    reader.GetString("batch_number"),
                    reader.GetDateTime("expiration_date"),
                    reader.GetInt32("quantity"),
                    reader.GetDecimal("unit_cost")
                );
            }
            return null;
        }

        public async Task<IEnumerable<Lot>> GetAll()
        {
            var list = new List<Lot>();
            string query = "SELECT * FROM lots";

            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Lot(
                    reader.GetInt32("id"),
                    reader.GetInt32("medicine_id"),
                    reader.GetString("batch_number"),
                    reader.GetDateTime("expiration_date"),
                    reader.GetInt32("quantity"),
                    reader.GetDecimal("unit_cost")
                ));
            }
            return list;
        }

        public async Task Update(Lot entity)
        {
            string query = @"UPDATE lots 
                            SET medicine_id=@medicine_id,
                                batch_number=@batch_number,
                                expiration_date=@expiration_date,
                                quantity=@quantity,
                                unit_cost=@unit_cost
                            WHERE id=@id";
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@medicine_id", entity.MedicineId);
            cmd.Parameters.AddWithValue("@batch_number", entity.BatchNumber);
            cmd.Parameters.AddWithValue("@expiration_date", entity.ExpirationDate);
            cmd.Parameters.AddWithValue("@quantity", entity.Quantity);
            cmd.Parameters.AddWithValue("@unit_cost", entity.UnitCost);
            cmd.Parameters.AddWithValue("@id", entity.Id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(int id)
        {
            string query = "DELETE FROM lots WHERE id=@id";
            using var connection = _db.GetConnection();
            await connection.OpenAsync();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
