namespace Farmacia_Arqui_Soft.Models
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

        public Provider()
        {

        }

        public Provider(int id, string firstname, string lastname, string nit, string addres, string email, string phone, byte status, bool is_deleted = false)
        {
            this.id = id;
            this.firstName = firstname;
            this.lastName = lastname;
            this.nit = nit;
            this.address = addres;
            this.email = email;
            this.phone = phone;
            this.status = status;
            this.is_deleted = is_deleted;
        }


    }
}
