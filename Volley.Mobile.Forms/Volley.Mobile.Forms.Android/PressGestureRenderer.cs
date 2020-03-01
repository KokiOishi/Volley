using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(PressGestureRenderer))]

namespace Xamarin.Forms
{
    public class PressGestureRenderer : ButtonRenderer
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This constructor is obsolete as of version 2.5. Please use ViewRenderer(Context) instead.")]
        public PressGestureRenderer()
        {
        }

        public PressGestureRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                if (!e.NewElement.GestureRecognizers.Any())
                    return;

                if (e.NewElement.GestureRecognizers.Any(x => x is PressGestureRecognizer || x is ReleaseGestureRecognizer))
                    Control.Touch += Control_Touch;
            }
        }

        private void Control_Touch(object sender, TouchEventArgs e)
        {
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    foreach (var gesture in Element.GestureRecognizers.OfType<PressGestureRecognizer>())
                    {
                        if (!(gesture.Command is null))
                        {
                            gesture.Command.Execute(gesture.CommandParameter);
                        }
                    }
                    break;

                case MotionEventActions.Up:
                    foreach (var gesture in Element.GestureRecognizers.OfType<ReleaseGestureRecognizer>())
                    {
                        if (!(gesture.Command is null))
                        {
                            gesture.Command.Execute(gesture.CommandParameter);
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}