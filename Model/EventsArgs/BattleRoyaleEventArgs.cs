using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EventsArgs
{
    public class BattleRoyaleEventArgs : EventArgs
    {
        public int RowUp;
        public int RowDown;
        public int ColLeft;
        public int ColRight;
        public BattleRoyaleEventArgs(int rowUp, int rowDown, int colLeft, int colRight)
        {
            RowUp = rowUp;
            RowDown = rowDown;
            ColLeft = colLeft;
            ColRight = colRight;
        }
    }
}
