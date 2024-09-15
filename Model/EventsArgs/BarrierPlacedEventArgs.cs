using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EventsArgs
{
    public class BarrierPlacedEventArgs : EventArgs
    {
        public int X;
        public int Y;
        public BarrierPlacedEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
