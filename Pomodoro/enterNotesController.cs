using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Pomodoro
{
    public partial class enterNotesController : UIViewController
    {
        // list of notes the user enters
        public List<String> listOfNotes { get; set; }

        public enterNotesController(IntPtr handle) : base(handle)
        {
            this.listOfNotes = new List<String>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //actions by clicking done button
            doneNotesButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                string notesEntered = notesTextArea.Text;
                if (notesEntered == " ")
                {
                    var alert = UIAlertController.Create("Error!", "Enter valid notes", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
                listOfNotes.Add(notesEntered);
            };
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            // set the View Controller that’s powering the screen we’re
            // transitioning to

            var notesController = segue.DestinationViewController as notesController;

            //set the Table View Controller’s list of notes to the
            // list of notes

            if (notesController != null)
            {
                notesController.notes = listOfNotes;
            }
        }
    }
}