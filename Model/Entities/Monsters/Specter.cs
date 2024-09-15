using Model.Board;
using Model.EventsArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities.Monsters
{
    public class Specter : Monster
    {
        private int tick;
        public Specter(int id, int x, int y, GameBoard board, int azon) : base(id, x, y, board, azon) {
            tick = 0;
        }
        public bool fuggveny(int x, int y, int dir)
        {
            switch (dir)
            {
                case 0: //UP
                    x = x - 1;
                    break;
                case 1: //DOWN
                    x = x + 1;
                    break;
                case 2: //LEFT
                    y = y - 1;
                    break;
                case 3: //RIGHT
                    y = y + 1;
                    break;;
                default: y = y + 1; break;

            }
            if (!(board.Width - 1 == x || 0 == y || 0 == x || board.Height - 1 == y) && !CheckBombsAt(x, y))
            {
                if (!board.Board[x, y].Wall && !board.Board[x, y].Box && !CheckBombsAt(x, y))
                {
                    return true;
                }
                else
                {
                    return fuggveny(x,y,dir);
                }
            }
            else
            {
                return false;
            }
        }
        public override void Move()
        {
            if (!Alive) return;
            tick++;
            if (!Alive) return;
            bool moved = false;
            if (tick%2==0)
            {
                moved = true;
            }
            //if (Random.Shared.Next(100) < 5)
            //    dir = Random.Shared.Next(0, 4);

 
            int attempts = 0;
            List<int> triedDirs = new List<int>();

            while (!moved && attempts < 4)
            {
                int newY = Y;
                int newX = X;

                switch (dir)
                {
                    case 0: //UP
                        newX = X - 1;
                        break;
                    case 1: //DOWN
                        newX = X + 1;
                        break;
                    case 2: //LEFT
                        newY = Y - 1;
                        break;
                    case 3: //RIGHT
                        newY = Y + 1;
                        break;
                    default:
                        return;
                }

                if (!(board.Width-1==newX || 0==newY || 0==newX || board.Height-1==newY)  && !CheckBombsAt(newX, newY))
                {
                    if (board.Board[newX, newY].Wall || board.Board[newX, newY].Box)
                    {
                        if (fuggveny(newX, newY, dir))
                        {
                            X = newX;
                            Y = newY;
                            CheckPlayersAt(newX, newY);
                            moved = true;
                            if (board.isthereStorme(X, Y) == true)
                            {
                                this.Alive = false;
                                this.Kill();
                            }
                        }
                        else
                        {
                            triedDirs.Add(dir);
                            attempts++;
                            do
                            {
                                dir = Random.Shared.Next(0, 4);
                            } while (triedDirs.Contains(dir));
                        }
                    }
                    else
                    {
                        X = newX;
                        Y = newY;
                        CheckPlayersAt(newX, newY);
                        moved = true;
                        if (board.isthereStorme(X, Y) == true)
                        {
                            this.Alive = false;
                            this.Kill();
                        }
                    }
                }
                else
                {
                    triedDirs.Add(dir);
                    attempts++;
                    do
                    {
                        dir = Random.Shared.Next(0, 4);
                    } while (triedDirs.Contains(dir));
                }
            }
        }
    }

}
