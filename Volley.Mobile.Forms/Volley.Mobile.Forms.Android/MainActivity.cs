using System;
using System.Diagnostics;
using System.Threading;
using Android.App;
using Android.Content.PM;
using Android.OS;

using Prism;
using Prism.Ioc;
using Debug = System.Diagnostics.Debug;

namespace Volley.Mobile.Forms.Droid
{
    [Activity(Label = "Volley.Mobile.Forms", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(bundle);

                Xamarin.Forms.Forms.SetFlags("CarouselView_Experimental");
                Xamarin.Forms.Forms.Init(this, bundle);
                LoadApplication(new App(new AndroidInitializer()));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}
