using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan.ViewModel
{
    public class HomeViewEventArgs : EventArgs
    {
        public int rounds { get; set; } // scores (player 1, player2)
        public int map_num { get; set; }

        public HomeViewEventArgs(int rounds, int map_num)
        {
            this.rounds = rounds;
            this.map_num = map_num;
        }
    }
}
