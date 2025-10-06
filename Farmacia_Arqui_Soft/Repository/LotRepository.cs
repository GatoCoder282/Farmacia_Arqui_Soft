using Farmacia_Arqui_Soft.Data;
using Farmacia_Arqui_Soft.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Farmacia_Arqui_Soft.Interfaces;

namespace Farmacia_Arqui_Soft.Repositories
{
    public class LotRepository : IRepository<Lot>
    {
        private readonly MySqlConnection _connection;

        public LotRepository()
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

            await cmd.ExecuteNonQueryAsync();
            entity.id = (int)cmd.LastInsertedId;
            return entity;
        }

        public async Task<Lot> GetById(int id)
        {
            string query = "SELECT * FROM Lot WHERE id = @id";
            using var cmd = new MySqlCommand(query, _connection);
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
            var lista = new List<Lot>();
            string query = "SELECT * FROM Lot";
            using var cmd = new MySqlCommand(query, _connection);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Lot(
                    reader.GetInt32("id"),
                    reader.GetInt32("medicine_id"),
                    reader.GetString("batch_number"),
                    reader.GetDateTime("expiration_date"),
                    reader.GetInt32("quantity"),
                    reader.GetDecimal("unit_cost")
                ));
            }
            return lista;
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

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(int id)
        {
            string query = "DELETE FROM Lot WHERE id=@id";
            using var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
