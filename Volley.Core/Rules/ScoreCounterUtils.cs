using System;
using System.Collections.Generic;
using System.Text;

namespace Volley.Rules
{
    public static class ScoreCounterUtils
    {
        [Obsolete("Use Extensions.CreateFactory(Func<T>)", true)]
        public static IFactory<T> CreateFactory<T>(this Func<T> ctorFunc)
            => Extensions.CreateFactory(ctorFunc);
    }
}