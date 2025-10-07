namespace Farmacia_Arqui_Soft.Models
{
    public class User
    {
        #region Atributos
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int phone { get; set; }
        public string ci { get; set; }

      
        public bool is_deleted { get; set; } = false;
        #endregion

        #region Constructor
        public User() { }

        public User(int id, string username, string password, int phone, string ci, bool is_deleted = false)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.phone = phone;
            this.ci = ci;
            this.is_deleted = is_deleted;
        }
        #endregion
    }
}
