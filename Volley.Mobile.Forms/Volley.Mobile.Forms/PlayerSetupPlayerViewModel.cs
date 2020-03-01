using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Volley.Standalone.Players;
using Xamarin.Forms;

namespace Volley.Mobile.Forms
{
    public sealed class PlayerSetupPlayerViewModel : INotifyPropertyChanged
    {
        private string name;

        public PlayerSetupPlayerViewModel(string name, Color color)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Color = color;
        }

        public string Name { get => name; set => this.SetAndNotifyIfChanged(PropertyChanged, name, name = value, nameof(Name)); }
        public Color Color { get; set; }

        public ICommand CommandDetails { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}