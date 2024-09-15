using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Model.Entities;

namespace Model.EventsArgs
{
    public class GameOverEventArgs : EventArgs
    {
        #region Properties

        public List<Player>? DeadPlayerList { get; }

        #endregion

        #region Constructor

        public GameOverEventArgs( List<Player>? deadPlayerList)
        {
            DeadPlayerList = deadPlayerList;
        }

        #endregion
    }
}
