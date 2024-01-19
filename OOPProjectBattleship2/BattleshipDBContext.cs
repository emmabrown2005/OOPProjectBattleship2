using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProjectBattleship2
{
    public class BattleshipDbContext
    {
        private readonly string connectionString;

        public BattleshipDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void FetchData()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Example: Query players
                using (var command = new SqlCommand("SELECT * FROM Players", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Username: {reader["Username"]}");
                        }
                    }
                }

                // Similar queries for other tables...

                connection.Close();
            }
        }
    }
}