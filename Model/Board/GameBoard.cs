using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Model.Entities;
using Model.Entities.Monsters;
using Model.EventsArgs;

namespace Model.Board
{
    public class GameBoard
    {
        #region Fields

        private Player[] players = new Player[2];
        private List<Bomb> bombs;
        private List<Monster> monsters;
        private List<(int, int, int)> barriers;
        private System.Timers.Timer monsterMoveTimer;
        private System.Timers.Timer monsterMoveTimer2;
        private System.Timers.Timer? gameOverTimer;
        private System.Timers.Timer? gameTimer;
        private bool brStarted;
        private int brCooldown;
        private int brSize;
        private int brWidth;
        private int brHeight;
        private int brCount;

        #endregion

        #region Properties

        public int Counter { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public Cell[,] Board { get; private set; }
        public Player[] Players { get { return players; } }
        public List<Bomb> Bombs { get { return bombs; } }
        public List<Monster> Monsters {  get { return monsters; } }
        public List<(int, int, int)> Barriers { get { return barriers; } }
        public bool RestartRequested { get; set; } = false;
        public bool Over { get; private set; }

        #endregion

        #region Constructor

        public GameBoard(int height, int width, int monstercount)
        {
            Width = width;
            Height = height;
            Board = new Cell[Height, Width];
            Over = false;
            Counter = 60;       //mennyi idő után kezdődjön el a battle royale

            brStarted = false;  //battle royale kezdetet jelzi
            brCooldown = 10;    //hany mp után csökken 1-el a pálya
            brSize = 5;         //5x5 terület maradjon a végén
            brHeight = (Height - brSize) / 2;
            brWidth = (Width - brSize) / 2;
            brCount = 0;

            Random random = new();
            int boxNum = 30;
            int monsterCount = monstercount;

            List<(int, int)> posAv = new();
            bombs = new();
            monsters = new();
            barriers = new();

            players[0] = new Player(1, Width / 2, this);
            players[1] = new Player(height - 2, Width / 2, this);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Board[i, j] = new Cell(false, false);
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1 || i % 2 == 0 && j % 2 == 0)
                        Board[i, j].Wall = true;
                    else if (!((i == 1 || i == 2 || i == 3 || i == Height - 4 || i == Height - 3 || i == Height - 2) && (j == (Width / 2) - 2 || j == (Width / 2) - 1 || j == Width / 2 || j == (Width / 2) + 1 || j == (Width / 2) + 2)))
                        posAv.Add((i, j));
                }
            }

            for (int i = 0; i < boxNum; i++)
            {
                int posInd = random.Next(posAv.Count);
                (int x, int y) = posAv[posInd];
                Board[x, y].Box = true;
                posAv.RemoveAt(posInd);
            }

            for (int i = 0; i < monsterCount; i++)
            {
                int posInd = random.Next(posAv.Count);
                (int x, int y) = posAv[posInd];
                int ir = random.Next(0,4);
                if (ir == 0)
                {
                    monsters.Add(new DefaultMonster(i, x, y, this, 0));
                }
                else if (ir == 1)
                {
                    monsters.Add(new Specter(i, x, y, this, 1));
                }
                else if (ir == 2)
                {
                    monsters.Add(new Stalker(i, x, y, this, 2));
                }
                else if (ir == 3)
                {
                    monsters.Add(new Wonderer(i, x, y, this, 3));
                }
                posAv.RemoveAt(posInd);
            }
            
            monsterMoveTimer = new System.Timers.Timer(1000);
            monsterMoveTimer.Elapsed += OnMonsterMoveTimerElapsed;
            monsterMoveTimer.AutoReset = true;

            monsterMoveTimer2 = new System.Timers.Timer(800);
            monsterMoveTimer2.Elapsed += OnMonsterMoveTimerElapsed2;
            monsterMoveTimer2.AutoReset = true;

            gameOverTimer = new System.Timers.Timer(2000);
            gameOverTimer.Elapsed += GetResult;
            gameOverTimer.Enabled = false;

            gameTimer = new System.Timers.Timer(1000);
            gameTimer.Elapsed += TimerElapsed;
            gameTimer.AutoReset = true;
        }

        #endregion

        #region Private methods

        private void AffectPlayersAt(int x, int y)
        {
            foreach (Player p in players)
            {
                if (p.X == x && p.Y == y && p.Alive)
                {
                    p.Kill();
                }
            }
        }
        private void AffectBombsAt(int x, int y)
        {
            List<Bomb> marked = new List<Bomb>();
            foreach (Bomb b in bombs)
            {
                if (b.X == x && b.Y == y)
                {
                    marked.Add(b);
                }
            }
            foreach (Bomb b in marked)
            {
                ExplodeBomb(b);
            }
        }
        private void AffectMonstersAt(int x, int y)
        {
            foreach (Monster m in monsters)
            {
                if (m.X == x && m.Y == y && m.Alive)
                {
                    m.Kill();
                }
            }
        }
        private void OnMonsterMoveTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            foreach (Monster m in monsters)
                if (m.Alive && m.azon != 2)
                {
                    var oldCoords = (m.X, m.Y);
                    m.Move();
                    OnMonsterMoved(new MonsterMovedEventArgs(oldCoords, (m.X, m.Y), m.Id));
                }
        }
        private void OnMonsterMoveTimerElapsed2(object? sender, ElapsedEventArgs e)
        {
            foreach (Monster m in monsters)
                if (m.Alive && m.azon == 2)
                {
                    var oldCoords = (m.X, m.Y);
                    m.Move();
                    OnMonsterMoved(new MonsterMovedEventArgs(oldCoords, (m.X, m.Y), m.Id));
                }
        }
        private void TimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (gameTimer == null) return;

            Counter--;
            OnTimerUpdate(new EventArgs());

            if (Counter == 1 && !brStarted)
            {
                brStarted = true;
                for (int i = 0; i < Width; i++)
                {
                    Board[0, i].Storm = true;
                    Board[Height - 1, i].Storm = true;
                    AffectEntitiesAt(0, i);
                    AffectEntitiesAt(Height - 1, i);

                }
                for (int i = 1; i < Height; i++)
                {
                    Board[i, 0].Storm = true;
                    Board[i, Width - 1].Storm = true;
                    AffectEntitiesAt(i, 0);
                    AffectEntitiesAt(i, Width - 1);

                }
                brHeight--;
                brWidth--;
                brCount++;
                OnBattleRoyale(new BattleRoyaleEventArgs(0, Height - 1, 0, Width - 1));
                Counter = brCooldown;
            }
            if (Counter == 1 && brStarted)
            {
                if (brHeight > 0)
                {
                    for (int i = brCount; i < Width; i++)
                    {
                        Board[brCount, i].Storm = true;
                        Board[Height - 1 - brCount, i].Storm = true;
                        AffectEntitiesAt(brCount, i);
                        AffectEntitiesAt(Height - 1 - brCount, i);

                    }
                    brHeight--;
                }
                if (brWidth > 0)
                {
                    for (int i = 1; i < Height; i++)
                    {
                        Board[i, brCount].Storm = true;
                        Board[i, Width - 1 - brCount].Storm = true;
                        AffectEntitiesAt(i, brCount);
                        AffectEntitiesAt(i, Width - 1 - brCount);

                    }
                    brWidth--;
                }
                OnBattleRoyale(new BattleRoyaleEventArgs(brCount, Height - 1 - brCount, brCount, Width - 1 - brCount));
                brCount++;
                Counter = brCooldown;
                if (brWidth <= 0 && brHeight <= 0)
                {
                    gameTimer.Enabled = false;
                    Counter = 0;
                    OnTimerUpdate(new EventArgs());
                }
            }
        }
        private void GetResult(object? sender, ElapsedEventArgs e)
        {
            if (gameOverTimer == null || gameTimer == null)
                return;

            brStarted = false;
            Over = true;
            List<Player> deadPlayers = new();
            gameOverTimer.Enabled = false;
            foreach (Player p in players)
                if (!p.Alive)
                    deadPlayers.Add(p);
            monsterMoveTimer.Enabled = false;
            monsterMoveTimer2.Enabled = false;
            gameTimer.Enabled = false;
            OnGameOver(new GameOverEventArgs(deadPlayers));
        }

        #endregion

        #region Public methods

        public void ToggleTimer()
        {
            if (gameTimer == null)
                return;
            gameTimer.Enabled = true;
        }
        public bool isthereStorme(int x, int y)
        {
            if (Board[x,y].Storm == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void AffectEntitiesAt(int x, int y)
        {
            AffectPlayersAt(x, y);
            AffectMonstersAt(x, y);
            AffectBombsAt(x, y);
            if (Board[x, y].Box)
                Board[x, y].OnBoxExplosion(x, y, this);

            OnExplosion(new ExplosionEventArgs(x, y));
        }
        public void CleanBomb(List<(int,int)> coords)
        {
            foreach(var c in coords)
            {
                OnExplosionClear(new ExplosionEventArgs(c.Item1, c.Item2));
            }
        }
        public void Dispose()
        {
            monsterMoveTimer?.Stop();
            monsterMoveTimer?.Dispose();
            monsterMoveTimer2?.Stop();
            monsterMoveTimer2?.Dispose();
            gameOverTimer?.Stop();
            gameOverTimer?.Dispose();
            gameOverTimer = null;
            gameTimer?.Stop();
            gameTimer?.Dispose();
            gameTimer = null;
        }
        public void TurnOnMonsterMoves()
        {
            monsterMoveTimer.Enabled = true;
            monsterMoveTimer2.Enabled = true;
        }
        public void MovePlayer(int p, int dir)
        {
            var oldCoords = (players[p].X, players[p].Y);
            if (players[p].Move(dir))
                OnPlayerMoved(new PlayerMovedEventArgs(oldCoords, (players[p].X, players[p].Y), p));
        }
        public void PlaceBomb(int p)
        {
            int px = players[p].X;
            int py = players[p].Y;
            if(players[p].PlaceBomb())
                OnBombPlaced(new BombPlacedEventArgs(px, py));
        }
        public void PlaceBarrier(int p)
        {
            int px = players[p].X;
            int py = players[p].Y;
            if (players[p].PlaceBarrier(p))
                OnBarrierPlaced(new BarrierPlacedEventArgs(px, py));
        }
        public void ExplodeBomb(Bomb b)
        {
            List<Explosion> Explosions = new List<Explosion>();
            Bombs.Remove(b);
            for(int i = 0; i <= 3; i++)
            {
                Explosions.Add(new Explosion(b.X, b.Y, b.Range, i, this));
            }
            List<(int, int)> coorrds = new List<(int, int)>();
            foreach (Explosion item in Explosions)
            {
                foreach(var c in item.coords)
                {
                    coorrds.Add(c);
                }
            }
            CleanBomb(coorrds);
        }
        public void StartGameOverTimer()
        {
            if (gameOverTimer != null)
                gameOverTimer.Enabled = true;
        }

        #endregion

        #region Events

        public event EventHandler<GameOverEventArgs>? GameOver;
        public event EventHandler<PlayerMovedEventArgs>? PlayerMoved;
        public event EventHandler<BombPlacedEventArgs>? BombPlaced;
        public event EventHandler<PlayerUpdateEventArgs>? PlayerUpdate;
        public event EventHandler<MonsterMovedEventArgs>? MonsterMoved;
        public event EventHandler<ExplosionEventArgs>? Explosion;
        public event EventHandler<ExplosionEventArgs>? ExplosionClear;
        public event EventHandler<BarrierPlacedEventArgs>? BarrierPlaced;
        public event EventHandler<EventArgs>? TimerUpdate;
        public event EventHandler<BattleRoyaleEventArgs>? BattleRoyale;
        public void OnGameOver(GameOverEventArgs e)
        {
            if (!RestartRequested)
            {
                GameOver?.Invoke(this, e);
            }
        }
        public void OnPlayerMoved(PlayerMovedEventArgs e)
        {
            PlayerMoved?.Invoke(this, e);
        }
        public void OnBombPlaced(BombPlacedEventArgs e)
        {
            BombPlaced?.Invoke(this, e);
        }
        public void OnPlayerUpdate(PlayerUpdateEventArgs e)
        {
            PlayerUpdate?.Invoke(this, e);
        }
        public void OnMonsterMoved(MonsterMovedEventArgs e)
        {
            MonsterMoved?.Invoke(this, e);
        }
        public void OnExplosion(ExplosionEventArgs e)
        {
            Explosion?.Invoke(this, e);
        }
        public void OnExplosionClear(ExplosionEventArgs e)
        {
            ExplosionClear?.Invoke(this, e);
        }
        public void OnBarrierPlaced(BarrierPlacedEventArgs e)
        {
            BarrierPlaced?.Invoke(this, e);
        }
        public void OnTimerUpdate(EventArgs e)
        {
            TimerUpdate?.Invoke(this, e);
        }
        public void OnBattleRoyale(BattleRoyaleEventArgs e)
        {
            BattleRoyale?.Invoke(this, e);
        }

        #endregion
    }
}