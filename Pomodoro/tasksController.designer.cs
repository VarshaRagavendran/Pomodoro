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
    [Register ("tasksController")]
    partial class tasksController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addTaskButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tableOfTasks { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField taskTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addTaskButton != null) {
                addTaskButton.Dispose ();
                addTaskButton = null;
            }

            if (tableOfTasks != null) {
                tableOfTasks.Dispose ();
                tableOfTasks = null;
            }

            if (taskTextField != null) {
                taskTextField.Dispose ();
                taskTextField = null;
            }
        }
    }
}