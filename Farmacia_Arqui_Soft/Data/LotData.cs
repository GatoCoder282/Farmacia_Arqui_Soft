using System.Data;
using MySql.Data.MySqlClient;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Interfaces;

namespace Farmacia_Arqui_Soft.Data
{
    public class LotData : IRepository<Lot>
    {
        public DataTable GetAll()
        {
            var table = new DataTable();
            using var conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();
            string query = "SELECT * FROM lot";
            using var cmd = new MySqlCommand(query, conn);
            using var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(table);
            return table;
        }

        public Lot? GetById(int id)
        {
            using var conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();
            string query = "SELECT * FROM lot WHERE id=@id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Lot
                {
                    Id = reader.GetInt32("id"),
                    MedicineId = reader.GetInt32("medicine_id"),
                    BatchNumber = reader.GetString("batch_number"),
                    ExpirationDate = reader.GetDateTime("expiration_date"),
                    EntryDate = reader.GetDateTime("entry_date"),
                    Quantity = reader.GetInt32("quantity"),
                    UnitCost = reader.GetDecimal("unit_cost"),
                    Status = reader.GetBoolean("status")
                };
            }
            return null;
        }

        public void Create(Lot entity)
        {
            using var conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();
            string query = @"INSERT INTO lot (medicine_id, batch_number, expiration_date, quantity, unit_cost)
                             VALUES (@med, @batch, @exp, @qty, @cost)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@med", entity.MedicineId);
            cmd.Parameters.AddWithValue("@batch", entity.BatchNumber);
            cmd.Parameters.AddWithValue("@exp", entity.ExpirationDate);
            cmd.Parameters.AddWithValue("@qty", entity.Quantity);
            cmd.Parameters.AddWithValue("@cost", entity.UnitCost);
            cmd.ExecuteNonQuery();
        }

        public void Update(Lot entity)
        {
            using var conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();
            string query = @"UPDATE lot 
                             SET medicine_id=@med, batch_number=@batch, expiration_date=@exp,
                                 quantity=@qty, unit_cost=@cost, status=@status 
                             WHERE id=@id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@med", entity.MedicineId);
            cmd.Parameters.AddWithValue("@batch", entity.BatchNumber);
            cmd.Parameters.AddWithValue("@exp", entity.ExpirationDate);
            cmd.Parameters.AddWithValue("@qty", entity.Quantity);
            cmd.Parameters.AddWithValue("@cost", entity.UnitCost);
            cmd.Parameters.AddWithValue("@status", entity.Status);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();
            string query = "DELETE FROM lot WHERE id=@id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
