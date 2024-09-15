using Model.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities.Monsters
{
    public class Wonderer : Monster
    {
        int[,] szamok;
        (int, int)[,] osok;
        public Wonderer(int id, int x, int y, GameBoard board, int azon) : base(id, x, y, board, azon) {
            szamok = new int[board.Height, board.Width];
            dir = 20;
            osok = new (int, int)[board.Height, board.Width];
            for (int i = 0; i < board.Height; i++)
            {
                for (int o = 0; o < board.Height; o++)
                {
                    szamok[i, o] = 200;
                }
            }
        }
        private (int, int) back(int r, int t)
        {
            if (osok[r, t] == (X, Y))
            {
                return (r, t);
            }
            else
            {
                return back(osok[r, t].Item1, osok[r, t].Item2);
            }
        }
        public override void Move()
        {
            if (!Alive) return;
            if (dir == 20)
            {
                dir = Random.Shared.Next(0, 4);
            }
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

            if (!board.Board[newX, newY].Wall && !board.Board[newX, newY].Box && !CheckBombsAt(newX, newY))
            {
                int e = 0;
                bool[] eme = new bool[4] { false, false, false, false };
                if (!board.Board[X, Y+1].Wall && !board.Board[X, Y + 1].Box && !CheckBombsAt(X, Y+1))
                {
                    eme[0]= true;
                    e++;
                }
                if (!board.Board[X+1, Y].Wall && !board.Board[X+1, Y].Box && !CheckBombsAt(X+1, Y))
                {
                    eme[1] = true;
                    e++;
                }
                if (!board.Board[X, Y - 1].Wall && !board.Board[X, Y - 1].Box && !CheckBombsAt(X, Y - 1))
                {
                    eme[2] = true;
                    e++;
                }
                if (!board.Board[X-1, Y].Wall && !board.Board[X-1, Y].Box && !CheckBombsAt(X-1, Y))
                {
                    eme[3] = true;
                    e++;
                }
                if (e> 2)
                {
                    int he = Random.Shared.Next(0, 10);
                    if (he ==5)
                    {
                        int op = Random.Shared.Next(0, 4);
                        while (eme[op] == false)
                        {
                            op = Random.Shared.Next(0, 4);
                        }
                        int tt = 0, rr = 1;
                        if (op == 0)
                        {
                            tt = X;
                            rr = Y + 1;
                        }
                        if (op ==1)
                        {
                            tt = X+1;
                            rr = Y;
                        }
                        if (op ==2)
                        {
                            tt = X;
                            rr = Y-1;
                        }
                        if(op ==3)
                        {
                            tt = X-1;
                            rr = Y;
                        }
                        if (tt + 1 == X && rr == Y)
                        {
                            dir = 0;
                        }
                        else if (tt - 1 == X && rr == Y)
                        {
                            dir = 1;
                        }
                        else if (tt == X && rr + 1 == Y)
                        {
                            dir = 2;
                        }
                        else if (tt == X && rr - 1 == Y)
                        {
                            dir = 3;
                        }
                        X = tt; Y = rr;
                    }
                    else
                    {
                        int tt, rr;
                        (tt, rr) = Megyelget();
                        if (tt + 1 == X && rr == Y)
                        {
                            dir = 0;
                        }
                        else if (tt - 1 == X && rr == Y)
                        {
                            dir = 1;
                        }
                        else if (tt == X && rr + 1 == Y)
                        {
                            dir = 2;
                        }
                        else if (tt == X && rr - 1 == Y)
                        {
                            dir = 3;
                        }
                        X = tt; Y = rr;
                    }
                }
                else
                {
                    X = newX;
                    Y = newY;
                    CheckPlayersAt(newX, newY);
                    if (board.isthereStorme(X, Y) == true)
                    {
                        this.Alive = false;
                        this.Kill();
                    }
                }
            }
            else
            {
                int tt, rr;
                (tt, rr) = Megyelget();
                if (tt + 1 == X && rr == Y)
                {
                    dir = 0;
                }
                else if (tt - 1 == X && rr == Y)
                {
                    dir = 1;
                }
                else if (tt == X && rr + 1 == Y)
                {
                    dir = 2;
                }
                else if (tt == X && rr - 1 == Y)
                {
                    dir = 3;
                }
                X = tt; Y = rr;
            }
        }
        public (int, int) Megyelget()
        {
            for (int i = 0; i < board.Height; i++)
            {
                for (int o = 0; o < board.Height; o++)
                {
                    szamok[i, o] = 200;
                }
            }
            int mega = 1;
            int megb = 1;
            List<(int, int, int)> lista = new List<(int, int, int)>();
            lista.Add((X, Y, 0));
            szamok[X, Y] = 0;
            osok[X, Y] = (X, Y);
            bool megvan = true;
            while (lista.Count != 0 && megvan)
            {
                int a = lista[0].Item1;
                int b = lista[0].Item2;
                mega = a;
                megb = b;
                int c = lista[0].Item3;
                lista.RemoveAt(0);
                if (szamok[a - 1, b] == 200 && !board.Board[a - 1, b].Wall && !board.Board[a - 1, b].Box && !CheckBombsAt(a - 1, b))
                {
                    if ((board.Players[0].X == a - 1 && board.Players[0].Y == b) || (board.Players[1].X == a - 1 && board.Players[1].Y == b))
                    {
                        megvan = false;
                        mega = a - 1;
                        megb = b;
                        szamok[a - 1, b] = 0;
                        lista.Add((a - 1, b, c + 1));
                        osok[a - 1, b] = (a, b);
                    }
                    else
                    {
                        szamok[a - 1, b] = 0;
                        lista.Add((a - 1, b, c + 1));
                        osok[a - 1, b] = (a, b);
                    }

                }
                if (szamok[a + 1, b] == 200 && !board.Board[a + 1, b].Wall && !board.Board[a + 1, b].Box && !CheckBombsAt(a + 1, b))
                {
                    if ((board.Players[0].X == a + 1 && board.Players[0].Y == b) || (board.Players[1].X == a + 1 && board.Players[1].Y == b))
                    {
                        megvan = false;
                        mega = a + 1;
                        megb = b;
                        szamok[a + 1, b] = 0;
                        lista.Add((a + 1, b, c + 1));
                        osok[a + 1, b] = (a, b);
                    }
                    else
                    {
                        szamok[a + 1, b] = 0;
                        lista.Add((a + 1, b, c + 1));
                        osok[a + 1, b] = (a, b);
                    }
                }
                if (szamok[a, b - 1] == 200 && !board.Board[a, b - 1].Wall && !board.Board[a, b - 1].Box && !CheckBombsAt(a, b - 1))
                {
                    if ((board.Players[0].X == a && board.Players[0].Y == b - 1) || (board.Players[1].X == a && board.Players[1].Y == b - 1))
                    {
                        megvan = false;
                        mega = a;
                        megb = b - 1;
                        szamok[a, b - 1] = 0;
                        lista.Add((a, b - 1, c + 1));
                        osok[a, b - 1] = (a, b);
                    }
                    else
                    {
                        szamok[a, b - 1] = 0;
                        lista.Add((a, b - 1, c + 1));
                        osok[a, b - 1] = (a, b);
                    }
                }
                if (szamok[a, b + 1] == 200 && !board.Board[a, b + 1].Wall && !board.Board[a, b + 1].Box && !CheckBombsAt(a, b + 1))
                {
                    if ((board.Players[0].X == a && board.Players[0].Y == b + 1) || (board.Players[1].X == a && board.Players[1].Y == b + 1))
                    {
                        megvan = false;
                        mega = a;
                        megb = b + 1;
                        szamok[a, b + 1] = 0;
                        lista.Add((a, b + 1, c + 1));
                        osok[a, b + 1] = (a, b);
                    }
                    else
                    {
                        szamok[a, b + 1] = 0;
                        lista.Add((a, b + 1, c + 1));
                        osok[a, b + 1] = (a, b);
                    }
                }
            }
            return back(mega, megb);

        }
    }
}
