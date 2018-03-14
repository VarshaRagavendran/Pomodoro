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
    [Register ("notesController")]
    partial class notesController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addNotesButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView listOfAllNotesTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addNotesButton != null) {
                addNotesButton.Dispose ();
                addNotesButton = null;
            }

            if (listOfAllNotesTable != null) {
                listOfAllNotesTable.Dispose ();
                listOfAllNotesTable = null;
            }
        }
    }
}