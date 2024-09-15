using Model.Entities.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EventsArgs
{
    public class MonsterMovedEventArgs : EventArgs
    {
        #region Properties

        public (int, int) OldCoords;
        public (int, int) NewCoords;
        public int Pid;

        #endregion

        #region Constructor

        public MonsterMovedEventArgs((int, int) oldCoords, (int, int) newCoords, int pid)
        {
            OldCoords = oldCoords;
            NewCoords = newCoords;
            Pid = pid;
        }

        #endregion
    }
}
