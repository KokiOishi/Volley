using System;
using System.Runtime.CompilerServices;

namespace Volley
{
    public static class Validation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GraterThan(this in int value, string name, int minExcluding)
            => value > minExcluding ? value : throw new ArgumentOutOfRangeException(name, $"{name} must be grater than {minExcluding}!");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static TResult Throw<TResult>(Exception exception) => throw exception;
    }
}
