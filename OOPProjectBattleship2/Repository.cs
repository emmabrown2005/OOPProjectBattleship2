using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProjectBattleship2
{
    public class Repository
    {
        string connectionString = "Data Source=.;Initial Catalog=BattleshipDatabase2;Integrated Security=True;";

        public Repository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Methods for Players

        public List<Players> GetPlayers()
        {
            List<Players> players = new List<Players>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM Players", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Players player = new Players
                            {
                                Username = Convert.ToString(reader["Username"]),
                                Password = Convert.ToString(reader["Password"])
                            };

                            players.Add(player);
                        }
                    }
                }

                connection.Close();
            }

            return players;
        }

        public void AddPlayer(Players player)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("INSERT INTO Players (Username, Password) VALUES (@Username, @Password); SELECT SCOPE_IDENTITY();", connection))
                {
                    command.Parameters.AddWithValue("@Username", player.Username);
                    command.Parameters.AddWithValue("@Password", player.Password);
                }

                connection.Close();
            }
        }


        public void UpdatePlayer(Players player)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("UPDATE Players SET Username = @Username, Password = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Username", player.Username);
                    command.Parameters.AddWithValue("@Password", player.Password);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        public List<Games> GetGames()
        {
            List<Games> games = new List<Games>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM Games", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Games game = new Games
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Title = Convert.ToString(reader["Title"]),
                            CreatorFK = Convert.ToString(reader["CreatorFK"]),
                            OpponentFK = Convert.ToString(reader["OpponentFK"]),
                            Complete = Convert.ToBoolean(reader["Complete"])
                            // Add other properties as needed
                        };

                        games.Add(game);
                    }
                }
            }

            return games;
        }
        public int GetGameId()
        {
            List<Games> games = GetGames();

            // checks if there are any games
            if (games.Count > 0)
            {
                // Return the ID of the most recent/active game
                return games.First().ID;
            }

            // Returns 0 if there are no games
            return 0;
        }

        public bool ValidateAndSaveShipConfiguration(string username, string coordinate)
        {
            // PUT VALIDATION LOGIC
            // For simplicity, i assume the coordinate is valid

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("INSERT INTO GameShipConfigurations (Username, Coordinate) VALUES (@Username, @Coordinate)", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Coordinate", coordinate);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return true; // Return true for simplicity (CHANGE AFTER VALIDATION LOGIC)
        }
        // ... (Other methods)

        public string LaunchAttack(string username, string coordinate)
        {
            // check for hits or misses against GameShipConfigurations
            bool isHit = CheckForHit(username, coordinate);

            // Save the attack in the Attacks table
            SaveAttack(username, coordinate, isHit);

            // Return the outcome
            return isHit ? "Hit!" : "Miss!";
        }

        public List<AttackOutcome> GetPastAttacks()
        {
            List<AttackOutcome> pastAttacks = new List<AttackOutcome>();

            // Fetch past attacks from the Attacks table
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT Coordinate, Hit FROM Attacks", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string coordinate = reader["Coordinate"].ToString();
                            bool isHit = (bool)reader["Hit"];
                            pastAttacks.Add(new AttackOutcome { Coordinate = coordinate, IsHit = isHit });
                        }
                    }
                }

                connection.Close();
            }

            return pastAttacks;
        }

        public bool CheckForWinner()
        {
            // This checks if all positive attacks amount to the total size of all the ships

            // For demonstration purposes, assuming there is a fixed total size
            int totalShipsSize = 17; //total ships size
            int totalAttacks = GetTotalAttacks(); // MAKE METHOD FOR TOTAL ATTACKS

            return totalAttacks == totalShipsSize;
        }

        public void MarkGameAsComplete(int ID)
        {
            // marks the game as complete in the Games table
            // Maybe udate a complete column?

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("UPDATE Games SET Complete = 1 WHERE /* Add your condition here */", connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private bool CheckForHit(string username, string coordinate)
        {
            // check if the coordinate is a hit against GameShipConfigurations
            // This might involve querying the GameShipConfigurations table and comparing the coordinate

            // For demonstration purposes, assume a random hit or miss
            return new Random().Next(2) == 1;
        }

        private void SaveAttack(string username, string coordinate, bool isHit)
        {
            // Your logic to save the attack in the Attacks table
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("INSERT INTO Attacks (Username, Coordinate, Hit) VALUES (@Username, @Coordinate, @Hit)", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Coordinate", coordinate);
                    command.Parameters.AddWithValue("@Hit", isHit);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        public int GetCurrentTurn(string username, int gameId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Assuming you have a table 'Attacks' with columns 'gameFK' and 'username'
                using (var command = new SqlCommand("SELECT COUNT(*) FROM Attacks WHERE gameFK = @gameFK AND username = @username", connection))
                {
                    command.Parameters.AddWithValue("@gameFK", gameId);
                    command.Parameters.AddWithValue("@username", username);

                    // ExecuteScalar returns the number of attacks made, which corresponds to the current turn
                    int currentTurn = Convert.ToInt32(command.ExecuteScalar());

                    return currentTurn;
                }
            }
        }


        public void AddGame(Games game)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("INSERT INTO Games (Title, CreatorFK, OpponentFK, Complete) VALUES (@Title, @CreatorFK, @OpponentFK, @Complete); SELECT SCOPE_IDENTITY();", connection))
                {
                    command.Parameters.AddWithValue("@Title", game.Title);
                    command.Parameters.AddWithValue("@CreatorFK", game.CreatorFK);
                    command.Parameters.AddWithValue("@OpponentFK", game.OpponentFK);
                    command.Parameters.AddWithValue("@Complete", game.Complete);

                    // ExecuteScalar returns the ID of the newly inserted record
                    int ID = Convert.ToInt32(command.ExecuteScalar());

                    // Update the GameID property of the game object
                    game.ID = ID;
                }

                connection.Close();
            }
        }


        private int GetTotalAttacks()
        {
            // logic to get the total number of attacks from the Attacks table
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT COUNT(*) FROM Attacks", connection))
                {
                    int totalAttacks = (int)command.ExecuteScalar();
                    return totalAttacks;
                }
            }
        }
    }

    public class AttackOutcome
    {
        public string Coordinate { get; set; }
        public bool IsHit { get; set; }
    }

}
