using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProjectBattleship2
{
    public class GameScreen
    {

        //this method has to go through a list of cells
        //populate/fetched from the database and it will have to call the PrintCell method of each of them

        private List<Cell> cells;

        public GameScreen(List<Cell> cells)
        {
            this.cells = cells;
        }

        // This method has to go through a list of cells and call the PrintCell method of each of them
        public void PrintGrid()
        {
            // Display column labels (1, 2, 3, ..., 8)
            Console.Write("   ");
            for (char col = 'A'; col <= 'H'; col++)
            {
                Console.Write(col + " ");
            }
            Console.WriteLine();

            // Display row labels (1, 2, 3, ..., 8) and grid cells
            for (int row = 1; row <= 8; row++)
            {
                Console.Write($"{row} |");

                for (char col = 'A'; col <= 'H'; col++)
                {
                    // Find the corresponding cell in the list
                    Cell currentCell = cells.Find(cell => cell.GridCellNo == (row - 1) * 8 + (col - 'A' + 1));

                    // Print the cell content
                    if (currentCell != null)
                    {
                        currentCell.PrintCell();
                    }
                    else
                    {
                        Console.Write(" "); // Empty cell
                    }

                    Console.Write(" ");
                }

                Console.WriteLine();
            }
        }

        // Additional constructor for static polymorphism
        public GameScreen(List<Attacks> attacks)
        {
            cells = new List<Cell>();
            foreach (var attack in attacks)
            {
                cells.Add(new AttackCell(attack));
            }
        }

        public GameScreen(List<GameShipConfigurations> configurations)
        {
            cells = new List<Cell>();
            foreach (var config in configurations)
            {
                cells.Add(new ShipCell(config));
            }
        }

    }
}
    