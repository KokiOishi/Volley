using System;
using System.Collections.Generic;
using System.Text;
using Volley.Standalone.Players;

namespace Volley.Standalone.Stats
{
    public interface IStatItem<out T>
    {
        string Name { get; }

        string TextValue { get; }

        T Value { get; }
    }
}
