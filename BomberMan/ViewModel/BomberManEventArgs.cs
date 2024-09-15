using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan.ViewModel
{
    public class BomberManEventArgs : EventArgs
    {
        public int p1 { get; set; } // scores (player 1, player2)
        public int p2 { get; set; }

        public BomberManEventArgs(int pp1, int pp2)
        {
            p1 = pp1;
            p2 = pp2;
        }
    }
}
