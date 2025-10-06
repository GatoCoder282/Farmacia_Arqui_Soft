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
        public bool Status { get; set; } = true;
    }
}
