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
        UIKit.UILabel colonLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView firstNumberTime { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView fourthNumberTime { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel listOfTasks { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView secondNumberTime { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton startButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton stopButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tableOfTasks { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView thirdNumberTime { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (colonLabel != null) {
                colonLabel.Dispose ();
                colonLabel = null;
            }

            if (firstNumberTime != null) {
                firstNumberTime.Dispose ();
                firstNumberTime = null;
            }

            if (fourthNumberTime != null) {
                fourthNumberTime.Dispose ();
                fourthNumberTime = null;
            }

            if (listOfTasks != null) {
                listOfTasks.Dispose ();
                listOfTasks = null;
            }

            if (secondNumberTime != null) {
                secondNumberTime.Dispose ();
                secondNumberTime = null;
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

            if (thirdNumberTime != null) {
                thirdNumberTime.Dispose ();
                thirdNumberTime = null;
            }
        }
    }
}