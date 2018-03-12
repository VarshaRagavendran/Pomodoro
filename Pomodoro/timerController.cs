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

            // register tableOfTasks tableview component
            tableOfTasks.RegisterClassForCellReuse(typeof(UITableViewCell), taskHistoryCellID);
            tableOfTasks.Source = new TaskHistoryDataSource(this);

            //start and stop button should be disabled initially
            if (Tasks.Count == 0)
            {
                startButton.Enabled = false;
                stopButton.Enabled = false;
            }

            //actions by clicking addTaskButton
            addTaskButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                string taskEntered = taskTextField.Text;
                if (taskEntered == " ")
                {
                    var alert = UIAlertController.Create("Error!", "Enter valid tasks", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
                Tasks.Add(taskEntered);
                tableOfTasks.ReloadData();
                taskTextField.Text = " ";
                startButton.Enabled = listOfTasksNonEmpty();
            };

            //actions by clicking startButton
            startButton.TouchUpInside += (object sender, EventArgs e) =>
            {

            };

            //actions by clicking stopButton
            stopButton.TouchUpInside += (object send, EventArgs e) =>
            {


            };

        }

        /**
         * Helper method to verify if list of tasks is empty.
         */
        private bool listOfTasksNonEmpty()
        {
            if (Tasks.Count > 0)
            {
                return true;
            }
            return false;
        }

        /**
         * Loading task data into table view
         */
        class TaskHistoryDataSource : UITableViewSource
        {
            timerController controller;

            public TaskHistoryDataSource(timerController controller)
            {
                this.controller = controller;
            }

            public override nint RowsInSection(UITableView tableView, nint section)
            {
                return controller.Tasks.Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(timerController.taskHistoryCellID);

                int row = indexPath.Row;
                cell.TextLabel.Text = controller.Tasks[row];
                return cell;
            }
        }
    }
}