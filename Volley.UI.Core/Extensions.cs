using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Volley.UI
{
    public static class Extensions
    {
        public static void SetAndNotifyIfChanged<TValue,TSender>(this TSender sender, PropertyChangedEventHandler @event
            , ref TValue target, TValue after, string pName)
            where TSender : INotifyPropertyChanged
        {
            if (!Equals(target, after)) @event?.Invoke(sender, new PropertyChangedEventArgs(pName));
            target = after;
        }
    }
}
