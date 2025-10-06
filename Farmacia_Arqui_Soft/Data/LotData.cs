using System.Data;
using MySql.Data.MySqlClient;
using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Models;

namespace Farmacia_Arqui_Soft.Data
{
    public class LotData : IRepository<Lot>
    {
        private readonly MySqlConnection _connection;

        public LotData()
        {
            _connection = DatabaseConnection.Instance.Connection;
        }

        public async Task<Lot> Create(Lot entity)
        {
            string query = "INSERT INTO Lot (medicine_id, batch_number, expiration_date, quantity, unit_cost) VALUES (@medicine_id, @batch_number, @expiration_date, @quantity, @unit_cost)";
            using var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@medicine_id", entity.medicine_id);
            cmd.Parameters.AddWithValue("@batch_number", entity.batch_number);
            cmd.Parameters.AddWithValue("@expiration_date", entity.expiration_date);
            cmd.Parameters.AddWithValue("@quantity", entity.quantity);
            cmd.Parameters.AddWithValue("@unit_cost", entity.unit_cost);

            await _connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            _connection.Close();

            entity.id = (int)cmd.LastInsertedId;
            return entity;
        }

        public async Task<Lot> GetById(int id)
        {
            string query = "SELECT * FROM Lot WHERE id = @id";
            using var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@id", id);

            await _connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            Lot lot = null;
            if (await reader.ReadAsync())
            {
                lot = new Lot
                {
                    id = reader.GetInt32("id"),
                    medicine_id = reader.GetInt32("medicine_id"),
                    batch_number = reader.GetString("batch_number"),
                    expiration_date = reader.GetDateTime("expiration_date"),
                    quantity = reader.GetInt32("quantity"),
                    unit_cost = reader.GetDecimal("unit_cost")
                };
            }

            _connection.Close();
            return lot;
        }

        public async Task<IEnumerable<Lot>> GetAll()
        {
            var list = new List<Lot>();
            string query = "SELECT * FROM Lot";
            using var cmd = new MySqlCommand(query, _connection);

            await _connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Lot
                {
                    id = reader.GetInt32("id"),
                    medicine_id = reader.GetInt32("medicine_id"),
                    batch_number = reader.GetString("batch_number"),
                    expiration_date = reader.GetDateTime("expiration_date"),
                    quantity = reader.GetInt32("quantity"),
                    unit_cost = reader.GetDecimal("unit_cost")
                });
            }

            _connection.Close();
            return list;
        }

        public async Task Update(Lot entity)
        {
            string query = "UPDATE Lot SET medicine_id=@medicine_id, batch_number=@batch_number, expiration_date=@expiration_date, quantity=@quantity, unit_cost=@unit_cost WHERE id=@id";
            using var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@medicine_id", entity.medicine_id);
            cmd.Parameters.AddWithValue("@batch_number", entity.batch_number);
            cmd.Parameters.AddWithValue("@expiration_date", entity.expiration_date);
            cmd.Parameters.AddWithValue("@quantity", entity.quantity);
            cmd.Parameters.AddWithValue("@unit_cost", entity.unit_cost);
            cmd.Parameters.AddWithValue("@id", entity.id);

            await _connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            _connection.Close();
        }

        public async Task Delete(int id)
        {
            string query = "DELETE FROM Lot WHERE id=@id";
            using var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@id", id);

            await _connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            _connection.Close();
        }
    }
}
