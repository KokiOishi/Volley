using System;

using AppKit;
using Foundation;
using Volley.Cocoa.Ui;

namespace Volley.Cocoa
{
    public partial class ViewController : NSViewController
    {
        MainModel mainModel;
        public ViewController(IntPtr handle) : base(handle)
        {
            mainModel = new MainModel(this);
        }
        partial void APointClicked(NSObject sender) => mainModel.OnAWin();

        partial void BPointClicked(NSObject sender) => mainModel.OnBWin();

        public string PointA { get => LabelPointA.StringValue; set => LabelPointA.StringValue = value; }
        public string PointB { get => LabelPointB.StringValue; set => LabelPointB.StringValue = value; }

        public string GameA { get => LabelGameA.StringValue; set => LabelGameA.StringValue = value; }
        public string GameB { get => LabelGameB.StringValue; set => LabelGameB.StringValue = value; }

        public string SetA { get => LabelSetA.StringValue; set => LabelSetA.StringValue = value; }
        public string SetB { get => LabelSetB.StringValue; set => LabelSetB.StringValue = value; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            mainModel.OnLoad();
            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
