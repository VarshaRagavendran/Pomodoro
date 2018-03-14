using Foundation;
using System;
using UIKit;

namespace Pomodoro
{
    
    public partial class notesController : UIViewController
    {
        public notesController (IntPtr handle) : base (handle)
        {
        }

        [Action("UnwindToNotesController:")]
        public void UnwindToNotesController (UIStoryboardSegue seguq)
        {}
    }
}