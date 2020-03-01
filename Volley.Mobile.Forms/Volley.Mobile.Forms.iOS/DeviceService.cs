using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Xamarin.Forms;
using Xamarin.Essentials;
using Volley.Mobile.Forms.iOS;
using System.Diagnostics;

[assembly: Dependency(typeof(DeviceService))]

namespace Volley.Mobile.Forms.iOS
{
    public class DeviceService : IDeviceService
    {
        public void Vibrate(double duration = 100)
        {
            try
            {
                Vibration.Vibrate(TimeSpan.FromMilliseconds(duration));
            }
            catch (FeatureNotSupportedException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
