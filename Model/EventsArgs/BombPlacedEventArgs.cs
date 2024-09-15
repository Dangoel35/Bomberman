using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EventsArgs
{
    public class BombPlacedEventArgs
    {
        public int X;
        public int Y;
        public BombPlacedEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
