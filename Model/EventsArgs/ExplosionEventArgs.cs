using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EventsArgs
{
    public class ExplosionEventArgs
    {
        public int X;
        public int Y;
        public ExplosionEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

