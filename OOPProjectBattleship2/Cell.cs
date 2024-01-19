using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProjectBattleship2
{
    public abstract class Cell
    {
        //common properties for both a cell showing a ship or a successful/missed attack
        //e.g public int GridCellNo;

        //this has to be implemented differently in the next two classes shown here
        //for a cell representing a ship or a cell representing a successful attack or missed attack
        //e.g if it is a ship, on screen print s; if it a miss on screen print O;
        // if it is a successful attack, on screen print X
        public int GridCellNo { get; set; }

        // For a cell representing a ship, print 'S'
        // For a cell representing a successful attack, print 'X'
        // For a cell representing a missed attack, print 'O'
        public abstract void PrintCell();
    }
}