using System;
using System.Collections.Generic;
using System.Text;
using Volley.Players;
using Volley.Standalone.Players;
using Xamarin.Forms;

namespace Volley.Mobile.Forms
{
    public class PlayerModel
    {
        public PlayerModel(PlayerStandalone player)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            Color = new Color(player.Color.Red, player.Color.Green, player.Color.Blue);
        }

        public PlayerStandalone Player { get; }
        public Color Color { get; }

        public override string ToString() => $"{base.ToString()}({nameof(Player)}: {Player.ToString()}, {nameof(Color)}: {Color.ToString()})";
    }
}