using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProjectBattleship2
{
    public class AttackCell : Cell
    {
        private Attacks attacks;

        public AttackCell(Attacks attacks)
        {
            this.attacks = attacks;
        }

        // Implement the PrintCell method for AttackCell
        public override void PrintCell()
        {
            Console.Write("X");
        }
    }
}