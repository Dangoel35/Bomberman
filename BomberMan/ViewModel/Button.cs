using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan.ViewModel
{
    public class Button : INotifyPropertyChanged
    {
        private int _x;
        public int X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }
        private int _y;
        public int Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
        private string? imageSource = "";
        public string? ImageSource
        {
            get => imageSource;
            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    OnPropertyChanged(nameof(ImageSource));
                }                
            }
        }
        private string? overlaySource = "";
        public string? OverlaySource
        {
            get => overlaySource;
            set
            {
                if (overlaySource != value)
                {
                    overlaySource = value;
                    OnPropertyChanged(nameof(OverlaySource));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}