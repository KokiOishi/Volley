using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using View = Xamarin.Forms.View;

[assembly: ExportRenderer(typeof(View), typeof(PressGestureRenderer))]

namespace Xamarin.Forms
{
    public class PressGestureRenderer : ViewRenderer<View, FrameworkElement>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                if (!e.NewElement.GestureRecognizers.Any())
                    return;

                if (e.NewElement.GestureRecognizers.Any(x => x is PressGestureRecognizer))
                    Control.PointerPressed += Control_PointerPressed;

                if (e.NewElement.GestureRecognizers.Any(x => x is ReleaseGestureRecognizer))
                    Control.PointerReleased += Control_PointerReleased;
            }
        }

        private void Control_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            foreach (var gesture in Element.GestureRecognizers.OfType<PressGestureRecognizer>())
            {
                if (!(gesture.Command is null))
                {
                    gesture.Command.Execute(gesture.CommandParameter);
                }
            }
        }

        private void Control_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            foreach (var gesture in Element.GestureRecognizers.OfType<ReleaseGestureRecognizer>())
            {
                if (!(gesture.Command is null))
                {
                    gesture.Command.Execute(gesture.CommandParameter);
                }
            }
        }
    }
}