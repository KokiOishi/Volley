using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Volley.Mobile.Forms.Droid;
using Xamarin.Forms;
using Xamarin.Essentials;

[assembly: Dependency(typeof(DeviceService))]

namespace Volley.Mobile.Forms.Droid
{
    public class DeviceService : IDeviceService
    {
        public void Vibrate(double duration = 100)
        {
            Vibration.Vibrate(duration);
        }
    }
}