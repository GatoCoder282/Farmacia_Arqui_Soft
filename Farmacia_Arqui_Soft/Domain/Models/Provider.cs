namespace Farmacia_Arqui_Soft.Domain.Models
{
    public class Provider
    {
        public int id { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string? nit { get; set; }
        public string? address { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public byte status { get; set; } = 1;
        public bool is_deleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Provider() { }

        public Provider(int id, string firstName, string lastName, string? nit, string? address, string? email, string? phone, byte status, bool is_deleted = false)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.nit = nit;
            this.address = address;
            this.email = email;
            this.phone = phone;
            this.status = status;
            this.is_deleted = is_deleted;
        }
    }
}

