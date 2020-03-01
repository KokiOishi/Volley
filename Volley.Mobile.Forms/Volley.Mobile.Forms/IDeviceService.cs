using System;
using System.Collections.Generic;
using System.Text;

namespace Volley.Mobile.Forms
{
    public interface IDeviceService
    {
        void Vibrate(double duration = 100);
    }
}