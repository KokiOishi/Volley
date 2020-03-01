using System;

using Xamarin.Forms;

namespace Volley.Mobile.Forms.Controls
{
    public struct StatItem
    {
        public StatItem(Color color, double value)
        {
            Color = color;
            Value = value >= 0 ? value : throw new ArgumentOutOfRangeException($"");
        }

        public Color Color { get; set; }
        public double Value { get; set; }
    }
}