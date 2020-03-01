using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using View = Xamarin.Forms.View;

[assembly: ExportRenderer(typeof(View), typeof(PressGestureRenderer))]

namespace Xamarin.Forms
{
    public class PressGestureRenderer : ViewRenderer
    {
        private class TouchLabel : UIView
        {
            private readonly View element = null;

            public TouchLabel(View element) => this.element = element;

            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);
                foreach (var gesture in element.GestureRecognizers.OfType<PressGestureRecognizer>())
                {
                    if (!(gesture.Command is null))
                    {
                        gesture.Command.Execute(gesture.CommandParameter);
                    }
                }
            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);
                foreach (var gesture in element.GestureRecognizers.OfType<ReleaseGestureRecognizer>())
                {
                    if (!(gesture.Command is null))
                    {
                        gesture.Command.Execute(gesture.CommandParameter);
                    }
                }
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            if (Control == null)
                SetNativeControl(new TouchLabel(Element) { });

            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                if (!e.NewElement.GestureRecognizers.Any())
                    return;

                Control.UserInteractionEnabled = true;
            }
        }
    }
}