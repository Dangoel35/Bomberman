using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BomberMan.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        #region values
        string test = "kezdes";
        int slider_value = 2;
        string selected_map = "Map 1";
        #endregion
        #region properties
        public ICommand ToGame { get; }

        public ICommand Settings { get; }

        public event EventHandler<HomeViewEventArgs>? ToGameView;

        public event EventHandler? SettingsView;
        public ICommand Map1Selected { get; }
        public ICommand Map2Selected { get; }
        public ICommand Map3Selected { get; }

        public string Test
        {
            get => test;
            set
            {
                test = value;
                OnPropertyChanged(nameof(Test));
            }
        }
        public string SelectedMap
        {
            get => selected_map;
            set
            {
                selected_map = value;
                OnPropertyChanged(nameof(SelectedMap));
            }
        }

        public int SliderValue
        {
            get => slider_value;
            set
            {
                slider_value = value;
                OnPropertyChanged(nameof(SliderValue));
            }
        }
        #endregion

        #region constructor

        public HomeViewModel()
        {
            ToGame = new DelegateCommand(param => JumpToGame());
            Settings = new DelegateCommand(param => JumpToSettings());
            Map1Selected = new DelegateCommand(param => Map1());
            Map2Selected = new DelegateCommand(param => Map2());
            Map3Selected = new DelegateCommand(param => Map3());
        }
        #endregion

        #region methods
        private void JumpToGame()
        {
            //BomberManEventArgs args = new BomberManEventArgs(slider_value);
            //ToGameView(this, EventArgs.Empty); //invoke the event
            int map_num = int.Parse(SelectedMap.Substring(4));
            ToGameView?.Invoke(this, new HomeViewEventArgs(SliderValue, map_num));
        }
        private void JumpToSettings()
        {
            SettingsView?.Invoke(this, EventArgs.Empty);
        }
        private void Map1()
        {
            SelectedMap = "Map 1";
            OnPropertyChanged(nameof(SelectedMap));
        }
        private void Map2()
        {
            SelectedMap = "Map 2";
            OnPropertyChanged(nameof(SelectedMap));
        }
        private void Map3()
        {
            SelectedMap = "Map 3";
            OnPropertyChanged(nameof(SelectedMap));
        }
        #endregion

    }
}
