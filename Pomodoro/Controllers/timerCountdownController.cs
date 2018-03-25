using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using System.Threading.Tasks;
using System.Text;
using System.Timers;
using RadialProgress;
using System.Drawing;

namespace Pomodoro
{
    public partial class timerCountdownController : UIViewController
    {
        /**
         * Actions to be performed once current view is loaded
         */
        public override void ViewDidLoad()
        {
            var progressView = new RadialProgressView
            {
                Center = new PointF((float)View.Center.X, (float)(View.Center.Y - 100))
            };
            progressView.Value = 0.5f;
            View.AddSubview(progressView);
        }
    }
}