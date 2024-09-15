using BomberMan.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BomberMan.View
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        int[] lista;
        public GameView()
        {
            InitializeComponent();
            lista = new int[12];
            using (StreamReader reader = new StreamReader("..\\..\\..\\Config\\adatok.txt"))
            {
                // Soronkénti beolvasás és feldolgozás
                string? line;
                for (int i = 0; i < 12; i++)
                {
                    line = reader.ReadLine();
                    lista[i] = Convert.ToInt32(line);
                }
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            Window_KeyDown(sender, e);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var vm = DataContext as BombermanViewModel;
            if (vm == null) return;

            switch (e.Key)
            {
                case Key.A:
                    //vm.MovePlayer(0, 0);
                    beallit(0);
                    break;
                case Key.W:
                    //vm.MovePlayer(0, 1);
                    beallit(1);
                    break;
                case Key.D:
                    beallit(2);
                    //vm.MovePlayer(0, 2);
                    break;
                case Key.S:
                    beallit(3);
                    //vm.MovePlayer(0, 3);
                    break;
                case Key.Right:
                    beallit(4);
                    //vm.PlaceBomb(0);
                    break;
                case Key.Left:
                    beallit(5);
                    //vm.PlaceBarrier(0);
                    break;
                case Key.Up:
                    beallit(6);
                    //vm.MovePlayer(1, 0);
                    break;
                case Key.Down:
                    beallit(7);
                    //vm.MovePlayer(1, 1);
                    break;
                case Key.E:
                    beallit(8);
                    //vm.MovePlayer(1, 2);
                    break;
                case Key.M:
                    beallit(9);
                    //vm.MovePlayer(1, 3);
                    break;
                case Key.N:
                    beallit(10);
                    //vm.PlaceBarrier(0);
                    break;
                case Key.F:
                    beallit(11);
                    //vm.PlaceBarrier(1);
                    break;
            }
        }
        private void beallit(int a)
        {
            var vm = DataContext as BombermanViewModel;
            if (vm == null) return;
            if (lista[a]==1)
            {
                //elore
                vm.MovePlayer(0, 0);
            }
            else if (lista[a] == 2)
            {
                //hatra
                vm.MovePlayer(0, 1);
            }
            else if (lista[a] == 3)
            {
                //ez most megy jobbra
                vm.MovePlayer(0, 3);
            }
            else if (lista[a] == 4)
            {
                //balra
                vm.MovePlayer(0, 2);
            }

            else if (lista[a] == 5) {
                vm.PlaceBomb(0);
            }
            else if (lista[a] == 6)
            {
                vm.PlaceBarrier(0);
            }
            else if (lista[a] == 7)
            {
                vm.MovePlayer(1, 0);
            }
            else if (lista[a] == 8)
            {
                vm.MovePlayer(1, 1);
            }
            else if (lista[a] == 9)
            {
                vm.MovePlayer(1, 3);
            }
            else if (lista[a] == 10)
            {
                vm.MovePlayer(1, 2);
            }
            else if (lista[a] == 11)
            {
                vm.PlaceBomb(1);
            }
            else if (lista[a] == 12)
            {
                vm.PlaceBarrier(1);
            }
        }
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
