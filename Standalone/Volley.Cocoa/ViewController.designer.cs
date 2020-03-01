// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Volley.Cocoa
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField LabelGameA { get; set; }

		[Outlet]
		AppKit.NSTextField LabelGameB { get; set; }

		[Outlet]
		AppKit.NSTextField LabelPointA { get; set; }

		[Outlet]
		AppKit.NSTextField LabelPointB { get; set; }

		[Outlet]
		AppKit.NSTextField LabelSetA { get; set; }

		[Outlet]
		AppKit.NSTextField LabelSetB { get; set; }

		[Action ("APointClicked:")]
		partial void APointClicked (Foundation.NSObject sender);

		[Action ("BPointClicked:")]
		partial void BPointClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (LabelPointA != null) {
				LabelPointA.Dispose ();
				LabelPointA = null;
			}

			if (LabelPointB != null) {
				LabelPointB.Dispose ();
				LabelPointB = null;
			}

			if (LabelGameA != null) {
				LabelGameA.Dispose ();
				LabelGameA = null;
			}

			if (LabelGameB != null) {
				LabelGameB.Dispose ();
				LabelGameB = null;
			}

			if (LabelSetA != null) {
				LabelSetA.Dispose ();
				LabelSetA = null;
			}

			if (LabelSetB != null) {
				LabelSetB.Dispose ();
				LabelSetB = null;
			}
		}
	}
}
