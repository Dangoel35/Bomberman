using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace BomberMan.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        List<string> alma;
        int[] lista;
        string almat = "";
        string ww = "";
        string ss = "";
        string dd = "";
        string le = "";
        string fel = "";
        string jobbra = "";
        string balra = "";
        string ee = "";
        string mm = "";
        string nn = "";
        string ff = "";
        public ICommand Back { get; }
        public event EventHandler? SettingsView;


        private void JumpToSettings()
        {
            StreamWriter writer = new StreamWriter("..\\..\\..\\Config\\adatok.txt");

            // Szöveg írása a fájlba
            for (int z = 0; z < lista.Count(); z++)
            {
                writer.WriteLine(lista[z]);
            }
            writer.Close();


            SettingsView?.Invoke(this, EventArgs.Empty);
        }

        public ICommand Press { get; set; }

        public string Almat
        {
            get => almat;
            set
            {
                almat = value;
                OnPropertyChanged(nameof(almat));
            }
        }
        public string WW
        {
            get => ww;
            set
            {
                ww = value;
                OnPropertyChanged(nameof(ww));
            }
        }
        public string SS
        {
            get => ss;
            set
            {
                ss = value;
                OnPropertyChanged(nameof(ss));
            }
        }
        public string DD
        {
            get => dd;
            set
            {
                dd = value;
                OnPropertyChanged(nameof(dd));
            }
        }
        public string FF
        {
            get => ff;
            set
            {
                ff = value;
                OnPropertyChanged(nameof(ff));
            }
        }
        public string NN
        {
            get => nn;
            set
            {
                nn = value;
                OnPropertyChanged(nameof(nn));
            }
        }
        public string EE
        {
            get => ee;
            set
            {
                ee = value;
                OnPropertyChanged(nameof(ee));
            }
        }
        public string MM
        {
            get => mm;
            set
            {
                mm = value;
                OnPropertyChanged(nameof(mm));
            }
        }
        public string Jobbra
        {
            get => jobbra;
            set
            {
                jobbra = value;
                OnPropertyChanged(nameof(jobbra));
            }
        }
        public string Balra
        {
            get => balra;
            set
            {
                balra = value;
                OnPropertyChanged(nameof(balra));
            }
        }
        public string Fel
        {
            get => fel;
            set
            {
                fel = value;
                OnPropertyChanged(nameof(fel));
            }
        }
        public string Le
        {
            get => le;
            set
            {
                le = value;
                OnPropertyChanged(nameof(le));
            }
        }

        public SettingsViewModel()
        {
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

            alma = new List<string>();
            alma.Add("");
            alma.Add("1. játékos előre");
            alma.Add("1. játékos hátra");
            alma.Add("1. játékos jobbra");
            alma.Add("1. játékos balra");
            alma.Add("1. játékos detonál");
            alma.Add("1. játékos aktival");
            alma.Add("2. játékos előre");
            alma.Add("2. játékos hátra");
            alma.Add("2. játékos jobbra");
            alma.Add("2. játékos balra");
            alma.Add("2. játékos detonál");
            alma.Add("2. játékos aktival");
            Press = new DelegateCommand(param => JumpToGame(param));
            Back = new DelegateCommand(param => JumpToSettings());
            Almat = alma[lista[Convert.ToInt64(0)]];
            WW = alma[lista[Convert.ToInt64(1)]];
            DD = alma[lista[Convert.ToInt64(2)]];
            SS = alma[lista[Convert.ToInt64(3)]];
            Jobbra = alma[lista[Convert.ToInt64(4)]];
            Balra = alma[lista[Convert.ToInt64(5)]];
            Fel = alma[lista[Convert.ToInt64(6)]];
            Le = alma[lista[Convert.ToInt64(7)]];
            EE = alma[lista[Convert.ToInt64(8)]];
            MM = alma[lista[Convert.ToInt64(9)]];
            NN = alma[lista[Convert.ToInt64(10)]];
            FF = alma[lista[Convert.ToInt64(11)]];
        }
        private void JumpToGame(object parameter) {

            if (lista[Convert.ToInt64(parameter.ToString())] < 12)
            {
                lista[Convert.ToInt64(parameter.ToString())]++;
            }
            else
            {
                lista[Convert.ToInt64(parameter.ToString())] = 0;
            }
            if (parameter.ToString()=="0")
            {
                Almat = alma[lista[Convert.ToInt64(parameter.ToString())]];
            }
            else if (parameter.ToString() == "1")
            {
                WW = alma[lista[Convert.ToInt64(parameter.ToString())]];
            }
            else if (parameter.ToString() == "2")
            {
                DD = alma[lista[Convert.ToInt64(parameter.ToString())]];
            }
            else if (parameter.ToString() == "3")
            {
                SS = alma[lista[Convert.ToInt64(parameter.ToString())]];
            }
            else if (parameter.ToString() == "4")
            {
                Jobbra = alma[lista[Convert.ToInt64(parameter.ToString())]];
            }
            else if (parameter.ToString() == "5")
            {
                Balra = alma[lista[Convert.ToInt64(parameter.ToString())]];
            }
            else if (parameter.ToString() == "6")
            {
                Fel = alma[lista[Convert.ToInt64(parameter.ToString())]];
            }
            else if (parameter.ToString() == "7")
            {
                Le = alma[lista[Convert.ToInt64(parameter.ToString())]];
            }
            else if (parameter.ToString() == "8")
            {
                EE = alma[lista[Convert.ToInt64(parameter.ToString())]];
            }
            else if (parameter.ToString() == "9")
            {
                MM = alma[lista[Convert.ToInt64(parameter.ToString())]];
            }
            else if (parameter.ToString() == "10")
            {
                NN = alma[lista[Convert.ToInt64(parameter.ToString())]];
            }
            else if (parameter.ToString() == "11")
            {
                FF = alma[lista[Convert.ToInt64(parameter.ToString())]];
            }
            //Test = alma[a];
        }

    }
}
