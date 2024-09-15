using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entities;
using Model.EventsArgs;

namespace Model.Board
{
    public class Cell
    {
        #region Fields

        private bool wall;
        private bool box;
        private bool storm;
        private Modifiers? powerUp;

        #endregion

        #region Properties

        public bool Wall { get { return wall; } set { wall = value; } }
        public bool Box { get { return box; } set { box = value; } }
        public bool Storm { get { return storm; } set { storm = value; } }
        public Modifiers? PowerUp { get { return powerUp; } set { powerUp = value; } }

        #endregion

        #region Constructor

        public Cell(bool wall, bool box)
        {
            this.wall = wall;
            this.box = box;
            storm = false;
            powerUp = null;
        }

        #endregion

        #region Public methods

        public void OnBoxExplosion(int x, int y, GameBoard board)
        {
            for (int i = 0; i < 2; i++) //Megnézi, hogy rendes doboz, vagy lerakott akadály-e
            {
                if (board.Barriers.Contains((x, y, i)))
                {
                    board.Barriers.Remove((x, y, i));
                    board.Players[i].AddBarrier();
                    box = false;
                    board.OnBarrierPlaced(new BarrierPlacedEventArgs(x, y));
                    return;
                }
            }
            Random random = new();
            int chance = random.Next(20);
            if (random.Next(2) == 1)                        //50%
            {
                if (chance < 3)
                    powerUp = Modifiers.PlusBombCapacity;   //15%
                else if (chance < 6)
                    powerUp = Modifiers.PlusBombRange;      //15%
                else if (chance < 8)
                    powerUp = Modifiers.Invincibility;      //10%
                else if (chance < 10)
                    powerUp = Modifiers.RollerSkates;       //10%
                else if (chance < 12)
                    powerUp = Modifiers.Ghost;              //10%
                else if (chance < 14)
                    powerUp = Modifiers.BlockBombPlacement; //10%              
                else if (chance < 16)
                    powerUp = Modifiers.Barrier;            //10%
                else if (chance == 16)
                    powerUp = Modifiers.Detonator;          //5%
                else if (chance == 17)
                    powerUp = Modifiers.MoveSpeedDown;      //5%
                else if (chance == 18)
                    powerUp = Modifiers.BombRush;           //5%
                else if (chance == 19)
                    powerUp = Modifiers.MinusBombRange;     //5%
            }
            box = false;
        }

        #endregion
    }
}
