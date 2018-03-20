// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Pomodoro
{
    [Register ("timerController")]
    partial class timerController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel hoursLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView hoursSelection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel listOfTasks { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel minutesLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView minutesSelection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel secondsLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView secondsSelection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton startButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton stopButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tableOfTasks { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (hoursLabel != null) {
                hoursLabel.Dispose ();
                hoursLabel = null;
            }

            if (hoursSelection != null) {
                hoursSelection.Dispose ();
                hoursSelection = null;
            }

            if (listOfTasks != null) {
                listOfTasks.Dispose ();
                listOfTasks = null;
            }

            if (minutesLabel != null) {
                minutesLabel.Dispose ();
                minutesLabel = null;
            }

            if (minutesSelection != null) {
                minutesSelection.Dispose ();
                minutesSelection = null;
            }

            if (secondsLabel != null) {
                secondsLabel.Dispose ();
                secondsLabel = null;
            }

            if (secondsSelection != null) {
                secondsSelection.Dispose ();
                secondsSelection = null;
            }

            if (startButton != null) {
                startButton.Dispose ();
                startButton = null;
            }

            if (stopButton != null) {
                stopButton.Dispose ();
                stopButton = null;
            }

            if (tableOfTasks != null) {
                tableOfTasks.Dispose ();
                tableOfTasks = null;
            }
        }
    }
}