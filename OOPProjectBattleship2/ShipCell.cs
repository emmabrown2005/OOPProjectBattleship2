using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProjectBattleship2
{
    public class ShipCell : Cell
    {
        private GameShipConfigurations configuration;

        public ShipCell(GameShipConfigurations configuration)
        {
            this.configuration = configuration;
        }

        // Implement the PrintCell method for ShipCell
        public override void PrintCell()
        {
            Console.Write("S");
        }
    }
}
