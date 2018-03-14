using Foundation;
using System;
using UIKit;

namespace Pomodoro
{
    public partial class mainPageController : UIViewController
    {
        public mainPageController (IntPtr handle) : base (handle)
        {}
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
           // this.NavigationItem.SetHidesBackButton (true, false);
            this.NavigationController.SetNavigationBarHidden(true, false);
        }

        [Action("UnwindToMainPageController:")]
        public void UnwindToMainPageController(UIStoryboardSegue seguq)
        { }
    }
}