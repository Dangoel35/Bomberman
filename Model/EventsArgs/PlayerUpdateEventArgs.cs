using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entities;

namespace Model.EventsArgs
{
    public class PlayerUpdateEventArgs : EventArgs
    {
        #region Properties

        public Player Player;

        #endregion

        #region Constructor

        public PlayerUpdateEventArgs(Player player) 
        {
            Player = player;
        }

        #endregion
    }
}
