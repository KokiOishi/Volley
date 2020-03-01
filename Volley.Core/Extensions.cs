using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Volley
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Raises <see cref="INotifyPropertyChanged.PropertyChanged"/> event when the <paramref name="value"/> differs from <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TSender"></typeparam>
        /// <param name="sender"></param>
        /// <param name="event"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="pName"></param>
        public static TValue SetAndNotifyIfChanged<TValue, TSender>(this TSender sender, PropertyChangedEventHandler @event
            , TValue target, TValue value, string pName)
            where TSender : INotifyPropertyChanged
        {
            if (!Equals(target, value)) @event?.Invoke(sender, new PropertyChangedEventArgs(pName));
            return value;
        }

        public static IFactory<T> CreateFactory<T>(this Func<T> ctorFunc)
            => new SimpleFactory<T>(ctorFunc ?? throw new ArgumentNullException(nameof(ctorFunc)));

        public static IParametricFactory<T, TParam> CreateParametricFactory<T, TParam>(this Func<TParam, T> ctorFunc)
                    => new SimpleParametrizedFactory<T, TParam>(ctorFunc ?? throw new ArgumentNullException(nameof(ctorFunc)));
    }
}