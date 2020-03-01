using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Volley.Mobile.Forms.Controls
{
    public sealed class StatModel
    {
        public StatModel(string name, Color color, double value, IEnumerable<StatItem> details)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Color = color;
            Value = value;
            Details = details ?? throw new ArgumentNullException(nameof(details));
        }

        public string Name { get; set; }
        public Color Color { get; set; }

        public double Value { get; set; }

        public IEnumerable<StatItem> Details { get; set; }
    }
}