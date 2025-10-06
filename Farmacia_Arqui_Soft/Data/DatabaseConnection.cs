using MySql.Data.MySqlClient;

namespace Farmacia_Arqui_Soft.Data
{

        public sealed class DatabaseConnection
        {
            private static DatabaseConnection _instance;
            private static readonly object _lock = new object();
            private MySqlConnection _connection;

            private DatabaseConnection()
            {
                string connectionString = "server=localhost;database=pharmacydb;user=root;password=1234;";
                _connection = new MySqlConnection(connectionString);
                _connection.Open();
            }

            public static DatabaseConnection Instance
            {
                get
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseConnection();
                        }
                        return _instance;
                    }
                }
            }

            public MySqlConnection Connection => _connection;
        }
}

