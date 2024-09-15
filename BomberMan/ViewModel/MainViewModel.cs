using BomberMan.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BomberMan.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; set; }

        public HomeViewModel HomeViewModel { get; } = new HomeViewModel();
        public EndViewModel EndViewModel { get; set; } = new EndViewModel(0, 0);
        public SettingsViewModel settingsmodel { get; } = new SettingsViewModel();
        //public BombermanViewModel BombermanViewModel { get; } = new BombermanViewModel(13, 13);
        public BombermanViewModel? BombermanViewModel { get; set; }

        EventHandler<HomeViewEventArgs> event_handler;
        EventHandler<BomberManEventArgs> event_handler2;
        EventHandler event_handler3;
        EventHandler eventmasikhandler;
        public MainViewModel()
        {
            event_handler = new EventHandler<HomeViewEventArgs>(ChangeView);
            event_handler2 = new EventHandler<BomberManEventArgs>(EndView);
            event_handler3 = new EventHandler(HomeView);
            eventmasikhandler = new EventHandler(ChangeSettins);
            //CurrentViewModel = BombermanViewModel
            CurrentViewModel = HomeViewModel;
            HomeViewModel.ToGameView += event_handler;
            //BombermanViewModel.ToEndView += event_handler2;
            EndViewModel.ToHomeView += event_handler3;
            //HomeViewModel.ToGameView += HandleEvent<BomberManEventArgs>;
            HomeViewModel.SettingsView += eventmasikhandler;
            settingsmodel.SettingsView += eventmasikhandler;

        }
        public void ChangeSettins(object? sender, EventArgs e)
        {
            if (CurrentViewModel == HomeViewModel)
            {
                CurrentViewModel = settingsmodel;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
            else
            {
                CurrentViewModel = HomeViewModel;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
        public void ChangeView(object? sender, HomeViewEventArgs e)
        {
            if (CurrentViewModel == HomeViewModel)
            {
                BombermanViewModel = new BombermanViewModel(13, 13, e.rounds, e.map_num);
                BombermanViewModel.ToEndView += event_handler2;
                CurrentViewModel = BombermanViewModel;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
            else
            {
                CurrentViewModel = HomeViewModel;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
        public void EndView(object? sender, BomberManEventArgs e)
        {
            EndViewModel.Player1Wins = e.p1;
            EndViewModel.Player2Wins = e.p2;
            CurrentViewModel = EndViewModel;
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        public void HomeView(object? sender, EventArgs e)
        {
            CurrentViewModel = HomeViewModel;
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        /*
        static void HandleEvent(object sender, BomberManEventArgs e)
        {
            Console.WriteLine("Event handler received message: " + e.Rounds);
        }*/

    }
}
