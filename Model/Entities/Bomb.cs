using Model.Board;
using System.Timers;

namespace Model.Entities
{
    public class Bomb
    {
        #region Fields

        private int delay;
        private readonly Player player;
        private readonly GameBoard board;
        private System.Timers.Timer? explosionTimer;

        #endregion

        #region Properties

        public TimeSpan SpreadDelay;
        public int Range { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        #endregion

        #region Constructor

        public Bomb(int x, int y, int range, TimeSpan spreadDelay, GameBoard board, Player player)
        {
            X = x;
            Y = y;
            delay = 2000;
            Range = range;
            this.board = board;
            this.player = player;
            SpreadDelay = spreadDelay;
            board.Bombs.Add(this);
            player.Bombs.Add(this);

            if (!player.HasDetonator)
            {
                explosionTimer = new System.Timers.Timer(delay);
                explosionTimer.Elapsed += OnExplosionTimerElapsed;
                explosionTimer.AutoReset = false;
                explosionTimer.Start();
            }
        }

        #endregion

        #region Public methods

        public void OnExplosionTimerElapsed(object? sender, ElapsedEventArgs? e)
        {
            if (explosionTimer == null || board.Over)
                return;
            board.ExplodeBomb(this);
            explosionTimer.Stop();
            explosionTimer.Dispose();
            player.AddBomb();
            if (player.HasBombRush)
                player.PlaceBomb();
        }


        #endregion
    }
}