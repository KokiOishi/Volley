using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Prism;
using Prism.Ioc;
using Prism.Logging;
using Volley.Mobile.Forms.ViewModels;
using Volley.Mobile.Forms.Views;
using Volley.Mobile.Forms.Views.Rules;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Volley.Mobile.Forms
{
    public partial class App
    {
        /*
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor.
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        public App() : this(null)
        {
            //Xamarin.Forms.Flags.SetFlags("CollectionView_Experimental");
        }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
                InitializeComponent();

                _ = await NavigationService.NavigateAsync("NavigationPage/MatchSetupPage");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception.ToString());
            throw e.Exception as Exception;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.ExceptionObject.ToString());
            throw e.ExceptionObject as Exception;
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            containerRegistry.RegisterSingleton<ILoggerFacade, DebugLogger>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            try
            {
                containerRegistry.RegisterForNavigation<NavigationPage>();
                containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
                containerRegistry.RegisterForNavigation<InitialPage>();
                containerRegistry.RegisterForNavigation<MatchSetupPage, MatchSetupViewModel>();
                containerRegistry.RegisterForNavigation<AdvantageSetSetupPage, AdvantageSetSetupPageViewModel>();
                containerRegistry.RegisterForNavigation<PlayerSetupPage, PlayerSetupPageViewModel>();
                containerRegistry.RegisterForNavigation<TieBreakSetSetupPage, TieBreakSetSetupPageViewModel>();
                containerRegistry.RegisterForNavigation<PlayerDetailsPage, PlayerDetailsPageViewModel>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }
    }
}
