using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Volley.Standalone
{
    /// <summary>
    /// Represents a linear RGB color with Bt.2020 gamut.
    /// </summary>
    public readonly struct Color
    {
        private readonly Vector3 values;

        public Color(Vector3 values)
        {
            this.values = values;
        }

        public Color(float r, float g, float b)
        {
            values = new Vector3(r, g, b);
        }

        public float Red => values.X;
        public float Green => values.Y;
        public float Blue => values.Z;
    }
}