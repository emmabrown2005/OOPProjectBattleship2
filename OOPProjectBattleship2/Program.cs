using System;
using System.Collections.Generic;
using System.Data.SqlClient; // Add this for SqlConnection
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProjectBattleship2
{
    class Program
    {
        static void Main()
        {
            // Assuming you have a List<Cell> containing ship configurations and past attacks
            List<Cell> cells = GetShipConfigurationsAndAttacks();

            // Create a GameScreen object and pass the cells list
            GameScreen gameScreen = new GameScreen(cells);

            // Display the grid
            gameScreen.PrintGrid();

            string connectionString = "Data Source=.;Initial Catalog=BattleshipDatabase;Integrated Security=True;";
            Repository repository = new Repository(connectionString);

            string gameTitle = "Battleship Game";

            string username1 = null, username2 = null;

            // Menu loop
            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Add Player details");
                Console.WriteLine("2. Configure Ships");
                Console.WriteLine("3. Launch Attack");
                Console.WriteLine("4. Quit");

                Console.Write("Enter your choice (1-4): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddPlayers(repository, ref username1, ref username2);
                        AssignPlayersToGame(repository, gameTitle, username1, username2);
                        break;
                    case "2":
                        if (username1 == null || username2 == null)
                        {
                            Console.WriteLine("Error: Players need to be added before configuring ships.");
                        }
                        else
                        {
                            ConfigureShips(repository, username1, username2);
                        }
                        break;
                    case "3":
                        // Implement Launch Attack logic
                        LaunchAttack(repository, username1);
                        break;
                    case "4":
                        Console.WriteLine("Exiting the game. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                        break;
                }
            }
        }

        static void AddPlayers(Repository repository, ref string username1, ref string username2)
        {
            Console.WriteLine("Adding Players:");

            Console.Write("Enter Player 1 Username: ");
            username1 = Console.ReadLine();
            Console.Write("Enter Player 1 Password: ");
            string password1 = MaskedInput();
            repository.AddPlayer(new Players { Username = username1, Password = password1 });

            Console.Write("Enter Player 2 Username: ");
            username2 = Console.ReadLine();
            Console.Write("Enter Player 2 Password: ");
            string password2 = MaskedInput();
            repository.AddPlayer(new Players { Username = username2, Password = password2 });
        }

        static void AssignPlayersToGame(Repository repository, string Title, string username1, string username2)
        {
            Console.WriteLine("Assigning Players to the Game:");

            repository.AddGame(new Games { Title = Title, CreatorFK = username1, OpponentFK = username2 });

            Console.WriteLine("Players assigned to the game.");
        }

        static void ConfigureShips(Repository repository, string username1, string username2)
        {
            Console.WriteLine("Configuring Ships:");

            ConfigureShipsForPlayer(repository, username1, "Player 1");
            ConfigureShipsForPlayer(repository, username2, "Player 2");

            Console.WriteLine("Ships configured for both players.");
        }

        static void ConfigureShipsForPlayer(Repository repository, string username, string v)
        {
            Console.WriteLine($"{username}, configure your ships:");

            for (int shipNumber = 1; shipNumber <= 5; shipNumber++)
            {
                Console.Write($"Enter the coordinate for Ship {shipNumber}: ");
                string coordinate = Console.ReadLine();

                // Validate and save ship configuration
                if (!repository.ValidateAndSaveShipConfiguration(username, coordinate))
                {
                    Console.WriteLine("Invalid input. Ships cannot overlap. Please try again.");
                    shipNumber--; // Repeat the current ship configuration
                }
            }
        }

        static string MaskedInput()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine(); // Move to the next line after masking input

            return password;
        }

        // ... (Other methods)

        static void LaunchAttack(Repository repository, string username)
        {
            // Determine whose turn it is
            int gameId = repository.GetGameId();
            int currentPlayerUsername = repository.GetCurrentTurn(username, gameId);

            Console.WriteLine($"Player {currentPlayerUsername}'s turn to launch an attack!");

            // Get the coordinate from the user
            Console.Write("Enter the attack coordinate (e.g., a1): ");
            string coordinate = Console.ReadLine();

            // Perform the attack using the repository
            var attackOutcome = repository.LaunchAttack(currentPlayerUsername.ToString(), coordinate);

            // Display the outcome
            Console.WriteLine($"Outcome: {attackOutcome}");

            // Display the grid of past attacks
            DisplayPastAttacksGrid(repository);

            // Check for a winner
            if (repository.CheckForWinner())
            {
                Console.WriteLine($"Player {currentPlayerUsername} is the winner!");
                // Mark the game as complete
                repository.MarkGameAsComplete(gameId);
            }
        }

        static void DisplayPastAttacksGrid(Repository repository)
        {
            // Fetch past attacks from the repository
            List<AttackOutcome> pastAttacks = repository.GetPastAttacks(); // Adjust based on your actual method

            Console.WriteLine("Past Attacks:");

            // Your logic to display past attacks as a grid (X for hits, O for misses)
            // You might want to format it similar to the game grid display
        }

        static char[,] gameGrid = new char[8, 7]; // Example 8x7 grid

        static void PrintGrid()
        {
            Console.Clear(); // Clear the console before printing the grid

            // Print column headers
            Console.Write("  ");
            for (char col = 'a'; col <= 'g'; col++)
            {
                Console.Write(col + " ");
            }
            Console.WriteLine();

            // Print grid with row headers
            for (int row = 0; row < gameGrid.GetLength(0); row++)
            {
                Console.Write((row + 1) + " ");
                for (int col = 0; col < gameGrid.GetLength(1); col++)
                {
                    Console.Write(gameGrid[row, col] + " ");
                }
                Console.WriteLine();
            }
        }

        static List<Cell> GetShipConfigurationsAndAttacks()
        {
            // Fetch ship configurations and attacks from the repository or other data source
            // Populate the List<Cell> accordingly

            // For demonstration purposes, create a sample list
            List<Cell> cells = new List<Cell>
            { };
            return cells;
        }

        // Other methods...
    }
}
