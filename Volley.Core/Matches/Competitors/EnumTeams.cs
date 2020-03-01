using System;
using System.Runtime.CompilerServices;

namespace Volley.Matches
{
    /// <summary>
    /// Initial service.
    /// </summary>
    public enum EnumTeams
    {
        /// <summary>
        /// The team a.
        /// </summary>
        TeamA,

        /// <summary>
        /// The team b.
        /// </summary>
        TeamB
    }

    /// <summary>
    /// Provides some functions to manipulate <see cref="EnumTeams"/>
    /// </summary>
    public static class EnumTeamsUtils
    {
        /// <summary>
        /// Flips the specified <see cref="EnumTeams"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EnumTeams Flip(this EnumTeams value) => value.Switch(EnumTeams.TeamB, EnumTeams.TeamA);

        /// <summary>
        /// Switches the specified two <typeparamref name="T"/> values by the specified <see cref="EnumTeams"/> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="valueA">The value to return when the <paramref name="value"/> is <see cref="EnumTeams.TeamA"/>.</param>
        /// <param name="valueB">The value to return when the <paramref name="value"/> is <see cref="EnumTeams.TeamB"/>.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Switch<T>(this EnumTeams value, T valueA, T valueB) => value == EnumTeams.TeamA ? valueA : valueB;
    }
}