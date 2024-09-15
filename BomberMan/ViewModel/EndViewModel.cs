using BomberMan.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BomberMan.ViewModel
{
    public class EndViewModel : ViewModelBase
    {
        public ICommand RestartGame { get; set; }
        public event EventHandler? ToHomeView;
        int player1Wins = 0;
        int player2Wins = 0;

        public int Player1Wins
        {
            get => player1Wins;
            set
            {
                player1Wins = value;
                OnPropertyChanged(nameof(Player1Wins));
            }
        }
        public int Player2Wins
        {
            get => player2Wins;
            set
            {
                player2Wins = value;
                OnPropertyChanged(nameof(Player2Wins));
            }
        }
        public EndViewModel(int p1, int p2)
        {
            Player1Wins = p1;
            Player2Wins = p2;
            OnPropertyChanged(nameof(Player1Wins));
            OnPropertyChanged(nameof(Player2Wins));
            RestartGame = new DelegateCommand(param => Restart());
        }

        private void Restart()
        {
            ToHomeView?.Invoke(this, EventArgs.Empty);
        }
    }
}