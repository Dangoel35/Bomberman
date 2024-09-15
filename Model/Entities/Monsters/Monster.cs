using Model.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities.Monsters
{
    public abstract class Monster
    {
        #region Fields

        protected int dir;
        protected readonly GameBoard board;

        #endregion

        #region Properties

        public int Id { get; protected set; }
        public int X { get; protected set; }
        public int azon { get; protected set; }
        public int Y { get; protected set; }
        public bool Alive { get; protected set; }

        #endregion

        #region Constructor

        public Monster(int id, int x, int y, GameBoard board, int azon)
        {
            Id = id;
            X = x;
            Y = y;
            dir = 1;
            Alive = true;
            this.board = board;
            this.azon = azon;
        }

        #endregion

        #region Protected methods

        protected bool CheckBombsAt(int x, int y)
        {
            foreach (Bomb b in board.Bombs)
                if (b.X == x && b.Y == y)
                    return true;
            return false;
        }
        protected void CheckPlayersAt(int x, int y)
        {
            foreach (Player p in board.Players)
                if (p.X == x && p.Y == y)
                    p.Kill();
        }

        #endregion

        #region Public methods

        public void Kill()
        {
            Alive = false;
        }
        public abstract void Move();

        #endregion
    }
}