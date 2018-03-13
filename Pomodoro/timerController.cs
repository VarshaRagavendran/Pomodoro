using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Pomodoro
{
    public partial class timerController : UIViewController
    {
        // list of tasks entered by the user
        public List<string> Tasks { get; set; }
        static NSString taskHistoryCellID = new NSString("TaskHistoryCell");

        public timerController(IntPtr handle) : base(handle)
        {
            Tasks = new List<string>();
        }

        /**
         * Actions to be performed once current view is loaded
         */
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            //actions by clicking startButton
            startButton.TouchUpInside += (object sender, EventArgs e) =>
            {

            };

            //actions by clicking stopButton
            stopButton.TouchUpInside += (object send, EventArgs e) =>
            {


            };

        }
    }
}