using Model.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Explosion
    {
        private int x, y;
        private int range, dir;
        private GameBoard board;
        public List<(int, int)> coords = new List<(int, int)> ();
        bool moving;
        public Explosion(int x, int y, int range, int dir, GameBoard board)
        {
            this.x = x;
            this.y = y;
            this.range = range;
            this.dir = dir;
            this.board = board;
            moving = true;
            coords.Add((x, y));
            Explode();
        }
        private void Move(int dir)
        {
            
            if (board.Board[x, y].Box || board.Board[x, y].Wall || board.Board[x, y].Storm)
            {
                moving = false;
            }
            if (Check() && moving)
            {
                switch (dir)
                {
                    case 0:
                        x--;
                        break;
                    case 1:
                        x++;
                        break;
                    case 2:
                        y--;
                        break;
                    case 3:
                        y++;
                        break;
                }
            }
            if (board.Board[x,y].Box || board.Board[x, y].Wall)
            {
                moving = false;
            }
            coords.Add((x, y));
        }
        private void Explode()
        {
            board.AffectEntitiesAt(x, y);
            for (int i = 0; i < range; i++)
            {
                Move(dir);
                board.AffectEntitiesAt(x, y);        
            }
            Thread.Sleep(100);
            //board.CleanBomb(coords);
        }
        private bool Check()
        {
            if(board.Height <= (x+1) || (x-1) < 0 || board.Width <= (y+1) || (y-1) < 0)
            {
                return false;
            }
            return true;
        }
    }
}
