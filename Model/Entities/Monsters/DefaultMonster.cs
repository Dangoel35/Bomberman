using Model.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities.Monsters
{
    public class DefaultMonster : Monster
    {
        public DefaultMonster(int id, int x, int y, GameBoard board, int azon) : base(id, x, y, board, azon) { }
        public override void Move()
        {
            if (!Alive) return;

            if (Random.Shared.Next(100) < 5)
                dir = Random.Shared.Next(0, 4);

            bool moved = false;
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

                if (!board.Board[newX, newY].Wall && !board.Board[newX, newY].Box && !board.Board[newX,newY].Storm && !CheckBombsAt(newX, newY))
                {
                    X = newX;
                    Y = newY;
                    CheckPlayersAt(newX, newY);
                    moved = true;
                    if (board.isthereStorme(X,Y)==true)
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
        }
    }
}
