namespace Farmacia_Arqui_Soft.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public byte Status { get; set; } = 1;

        public Lot() { }

        public Lot(int id, int medicineId, string batchNumber, DateTime expirationDate, int quantity, decimal unitCost, byte status)
        {
            Id = id;
            MedicineId = medicineId;
            BatchNumber = batchNumber;
            ExpirationDate = expirationDate;
            Quantity = quantity;
            UnitCost = unitCost;
            Status = status;
        }
    }
}
