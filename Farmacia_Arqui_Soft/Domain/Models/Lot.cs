namespace Farmacia_Arqui_Soft.Domain.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public bool is_deleted { get; set; } = false;

        public Lot() { }

        public Lot(int id, int medicineId, string batchNumber, DateTime expirationDate, int quantity, decimal unitCost, bool is_deleted = false)
        {
            Id = id;
            MedicineId = medicineId;
            BatchNumber = batchNumber;
            ExpirationDate = expirationDate;
            Quantity = quantity;
            UnitCost = unitCost;
            this.is_deleted = is_deleted;
        }
    }
}
