using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Model.Board;
using Model.Entities.Monsters;

namespace Model.Entities
{
    public class Player
    {
        #region Fields

        private readonly GameBoard board;
        private System.Timers.Timer? invincibilityTimer;
        private System.Timers.Timer? invincibilityOverTimer;
        private System.Timers.Timer? slowTimer;
        private System.Timers.Timer? rangeDecreaseTimer;
        private System.Timers.Timer? ghostTimer;
        private System.Timers.Timer? ghostOverTimer;
        private System.Timers.Timer? blockBombTimer;
        private System.Timers.Timer? bombRushTimer;
        private DateTime lastMoveTime = DateTime.MinValue;
        private TimeSpan spreadDelay = TimeSpan.FromSeconds(0.2);
        private TimeSpan moveCoolDown = TimeSpan.FromSeconds(0.15);       

        #endregion

        #region Properties

        public int X { get; private set; }
        public int Y { get; private set; }
        public int BombCount { get; private set; }
        public int MaxBombCount { get; private set; }
        public int BombRange { get; private set; }
        public int MaxBombRange { get; private set; }
        public int BarrierCount { get; private set; }
        public bool Alive { get; private set; }
        public bool HasRollerSkates { get; private set; }
        public bool HasInvincibility { get; private set; }
        public bool InvincibilityOver { get; private set; }
        public bool HasDetonator { get; private set; }
        public bool HasGhost { get; private set; }
        public bool GhostOver { get; private set; }
        public bool HasBombBlock { get; private set; }
        public bool HasBombRush { get; private set; }

        public List<Bomb> Bombs { get; private set; }
        #endregion

        #region Constructor

        public Player(int x, int y, GameBoard board)
        {
            X = x;
            Y = y;
            Alive = true;
            BombCount = 1;
            MaxBombCount = 1;
            BombRange = 2;
            MaxBombRange = 2;
            BarrierCount = 0;
            this.board = board;
            HasRollerSkates = false;
            HasInvincibility = false;
            InvincibilityOver = false;
            HasDetonator = false;
            HasGhost = false;
            GhostOver = false;
            HasBombBlock = false;
            HasBombRush = false;
            Bombs = new List<Bomb>();
        }

        #endregion

        #region Private methods

        private bool CheckBombsAt(int x, int y)
        {
            foreach (Bomb b in board.Bombs)
                if (b.X == x && b.Y == y)
                    return true;
            return false;
        }
        private void CheckMonstersAt(int x,int y)
        {
            foreach (Monster m in board.Monsters)
                if (x == m.X && m.Y == y && m.Alive)
                    Kill();
        }

        #region Power up dolgok

        private void CheckPowerUpsAt(int x, int y)
        {
            switch (board.Board[x, y].PowerUp)
            {
                case Modifiers.PlusBombCapacity:
                    BombCount++;
                    MaxBombCount++;
                    break;

                case Modifiers.PlusBombRange:
                    BombRange++;
                    MaxBombRange++;
                    break;

                case Modifiers.Invincibility:
                    GiveInvincibility();
                    break;

                case Modifiers.RollerSkates:
                    if (!HasRollerSkates)
                        GiveRollerBlades();
                    break;

                case Modifiers.MoveSpeedDown:
                    SlowMovement();
                    break;

                case Modifiers.MinusBombRange:
                    DecreaseRange();
                    break;

                case Modifiers.Ghost:
                    GiveGhost();
                    break;

                case Modifiers.BlockBombPlacement:
                    BlockBombPlacement();
                    break;

                case Modifiers.Barrier:
                    AddBarriers();
                    break;

                case Modifiers.BombRush:
                    GiveBombRush();
                    break;

                case Modifiers.Detonator:
                    GiveDetonator(); 
                    break;

                default:
                    break;
            }
            board.Board[x, y].PowerUp = null;
        }
        private void SlowMovement()
        {
            moveCoolDown = TimeSpan.FromSeconds(0.3);
            if (slowTimer != null)
            {
                slowTimer.Stop();
                slowTimer.Start();
            }
            else
            {
                slowTimer = new(10000);
                slowTimer.AutoReset = false;
                slowTimer.Elapsed += (sender, e) =>
                {
                    moveCoolDown = TimeSpan.FromSeconds(0.15);
                    slowTimer.Stop();
                    slowTimer.Dispose();
                    slowTimer = null;
                };
                slowTimer.Start();
            }
        }
        private void DecreaseRange()
        {
            BombRange = 1;
            if (rangeDecreaseTimer != null)
            {
                rangeDecreaseTimer.Stop();
                rangeDecreaseTimer.Start();
            }
            else
            {
                rangeDecreaseTimer = new(10000);
                rangeDecreaseTimer.AutoReset = false;
                rangeDecreaseTimer.Elapsed += (sender, e) =>
                {
                    BombRange = MaxBombRange;
                    rangeDecreaseTimer.Stop();
                    rangeDecreaseTimer.Dispose();
                    rangeDecreaseTimer = null;
                };
            }
        }
        private void GiveInvincibility()
        {
            HasInvincibility = true;
            InvincibilityOver = false;

            if (invincibilityTimer != null)
            {
                invincibilityTimer.Stop();
                invincibilityTimer.Start();
            }
            else
            {
                invincibilityTimer = new(10000);
                invincibilityTimer.AutoReset = false;
                invincibilityTimer.Elapsed += (sender, e) =>
                {
                    InvincibilityOver = false;
                    HasInvincibility = false;
                    board.OnPlayerUpdate(new EventsArgs.PlayerUpdateEventArgs(this));
                    invincibilityTimer.Stop();
                    invincibilityTimer.Dispose();
                    invincibilityTimer = null;
                };
                invincibilityTimer.Start();
            }

            if (invincibilityOverTimer != null)
            {
                invincibilityOverTimer.Stop();
                invincibilityOverTimer.Start();
            }
            else
            {
                invincibilityOverTimer = new System.Timers.Timer(7000)
                {
                    AutoReset = false
                };
                invincibilityOverTimer.Elapsed += (sender, e) =>
                {
                    InvincibilityOver = true;
                    board.OnPlayerUpdate(new EventsArgs.PlayerUpdateEventArgs(this));
                    invincibilityOverTimer.Dispose();
                    invincibilityOverTimer = null;
                };
                invincibilityOverTimer.Start();
            }

        }
        private void GiveRollerBlades()
        {
            HasRollerSkates = true;
            moveCoolDown = TimeSpan.FromSeconds(0.05);
        }
        private void GiveGhost()
        {
            HasGhost = true;
            GhostOver = false;

            if (ghostOverTimer != null)
            {
                ghostOverTimer.Stop();
                ghostOverTimer.Start();
            }
            else
            {
                ghostOverTimer = new System.Timers.Timer(7000)
                {
                    AutoReset = false
                };
                ghostOverTimer.Elapsed += (sender, e) =>
                {
                    GhostOver = true;
                    board.OnPlayerUpdate(new EventsArgs.PlayerUpdateEventArgs(this));
                    ghostOverTimer.Dispose();
                    ghostOverTimer = null;
                };
                ghostOverTimer.Start();
            }

            if (ghostTimer != null)
            {
                ghostTimer.Stop();
                ghostTimer.Start();
            }
            else
            {
                ghostTimer = new(10000);
                ghostTimer.AutoReset = false;
                ghostTimer.Elapsed += (sender, e) =>
                {
                    GhostOver = false;
                    HasGhost = false;
                    board.OnPlayerUpdate(new EventsArgs.PlayerUpdateEventArgs(this));
                    ghostTimer.Stop();
                    ghostTimer.Dispose();
                    ghostTimer = null;
                    if (board.Board[X, Y].Wall || board.Board[X, Y].Box)
                        Kill();
                };
                ghostTimer.Start();
            }
        }
        private void BlockBombPlacement()
        {
            HasBombBlock = true;
            if (blockBombTimer != null)
            {
                blockBombTimer.Stop();
                blockBombTimer.Start();
            }
            else
            {
                blockBombTimer = new(10000);
                blockBombTimer.AutoReset = false;
                blockBombTimer.Elapsed += (sender, e) =>
                {
                    HasBombBlock = false;
                    blockBombTimer.Stop();
                    blockBombTimer.Dispose();
                    blockBombTimer = null;
                };
                blockBombTimer.Start();
            }
        }
        private void AddBarriers()
        {
            BarrierCount += 3;
        }
        private void GiveBombRush()
        {
            HasBombRush = true;
            if (bombRushTimer != null)
            {
                bombRushTimer.Stop();
                bombRushTimer.Start();
            }
            else
            {
                bombRushTimer = new(5000);
                bombRushTimer.AutoReset = false;
                bombRushTimer.Elapsed += (sender, e) =>
                {
                    HasBombRush = false;
                    bombRushTimer.Stop();
                    bombRushTimer.Dispose();
                    bombRushTimer = null;
                };
                bombRushTimer.Start();
            }
        }
        private void GiveDetonator()
        {
            HasDetonator = true;
        }

        #endregion

        #endregion

        #region Public methods

        public bool Move(int dir)
        {
            if (!Alive || DateTime.Now - lastMoveTime < moveCoolDown)
                return false;

            int newX = X;
            int newY = Y;
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
                    return false;
            }
            if (board.Board[newX, newY].Storm && !(newX == board.Height || newX == -1) && !(newY == -1 || newY == board.Width)) 
                return false;

            if (HasGhost && !(newX == board.Height || newX == -1) && !(newY == -1 || newY == board.Width))
            {
                CheckPowerUpsAt(newX, newY);
                CheckMonstersAt(newX, newY);
                X = newX;
                Y = newY;
                lastMoveTime = DateTime.Now;
                if (HasBombRush)
                    PlaceBomb();

                return true;
            }
            else if (!HasGhost && !board.Board[newX, newY].Wall && !board.Board[newX, newY].Box && !CheckBombsAt(newX, newY))
            {
                CheckPowerUpsAt(newX, newY);
                CheckMonstersAt(newX, newY);
                X = newX;
                Y = newY;
                lastMoveTime = DateTime.Now;
                if (HasBombRush)
                    PlaceBomb();

                return true;
            }
            return false;
        }
        public bool PlaceBomb()
        {
            foreach (Bomb b in board.Bombs)
            {
                if (b.X == X && b.Y == Y)
                    return false;
            }
            if (BombCount > 0 && Alive && !HasBombBlock)
            {
                BombCount--;
                new Bomb(X, Y, BombRange, spreadDelay, board, this);
                return true;
            }
            
            if (BombCount == 0 && HasDetonator)
            {
                ExplodeAllBombs();
                for (int i = 0; i < MaxBombCount; i++)
                    AddBomb();

                return false;
            }
            return false;
        }
        public bool PlaceBarrier(int id)
        {
            if (BarrierCount > 0 && !board.Board[X, Y].Box && !board.Board[X, Y].Wall && Alive)
            {
                BarrierCount--;
                board.Barriers.Add((X, Y, id));
                board.Board[X, Y].Box = true;
                return true;
            }
            return false;
        }
        public void Kill()
        {
            if (HasInvincibility && !board.Board[X,Y].Storm)
                return;

            Alive = false;
            board.StartGameOverTimer();
            board.OnPlayerUpdate(new EventsArgs.PlayerUpdateEventArgs(this));
        }               
        public void AddBomb()
        {
            BombCount++;
        }
        public void AddBarrier()
        {
            BarrierCount++;
        }
        public void ExplodeAllBombs()
        {
            foreach (Bomb bomb in Bombs)
            {
                board.ExplodeBomb(bomb);
            }
            Bombs.Clear();
            if (HasBombRush)
                while (BombCount > 0)
                {
                    PlaceBomb();
                }
        }
        #endregion
    }
}