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
    [Register ("enterNotesController")]
    partial class enterNotesController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton doneNotesButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel notesLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView notesTextArea { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (doneNotesButton != null) {
                doneNotesButton.Dispose ();
                doneNotesButton = null;
            }

            if (notesLabel != null) {
                notesLabel.Dispose ();
                notesLabel = null;
            }

            if (notesTextArea != null) {
                notesTextArea.Dispose ();
                notesTextArea = null;
            }
        }
    }
}