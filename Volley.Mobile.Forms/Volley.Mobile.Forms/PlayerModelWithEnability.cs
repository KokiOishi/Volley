using System;
using System.Collections.Generic;
using System.Text;
using Volley.Players;
using Volley.Standalone.Players;
using Xamarin.Forms;

namespace Volley.Mobile.Forms
{
    public sealed class PlayerModelWithEnability : PlayerModel
    {
        public bool IsEnabled { get; }

        public PlayerModelWithEnability(PlayerStandalone player, bool isEnabled) : base(player)
        {
            IsEnabled = isEnabled;
        }
    }
}