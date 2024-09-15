using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EventsArgs
{
    public class PlayerMovedEventArgs : EventArgs
    {
        #region Properties

        public (int, int) OldCoords;
        public (int, int) NewCoords;
        public int PId;

        #endregion

        #region Constructor

        public PlayerMovedEventArgs((int, int) oldCoords, (int, int) newCoords, int pId)
        {
            OldCoords = oldCoords;
            NewCoords = newCoords;
            PId = pId;
        }

        #endregion
    }
}
