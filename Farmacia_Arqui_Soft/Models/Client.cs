namespace Farmacia_Arqui_Soft.Models
{
    public class Client
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string nit { get; set; }
        public string email { get; set; }

        public Client()
        {
            
        }

        public Client(int id, string first_name, string last_name, string nit, string email)
        {
            this.id = id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.nit = nit;
            this.email = email;
        }
    }
}
