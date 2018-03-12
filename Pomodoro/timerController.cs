using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Pomodoro
{
    public partial class timerController : UIViewController
    {
        public List<string> Tasks { get; set; }
        static NSString taskHistoryCellID = new NSString("TaskHistoryCell");

        public timerController (IntPtr handle) : base (handle)
        {
            tableOfTasks.RegisterClassForCellReuse(typeof(UITableViewCell), taskHistoryCellID);
            tableOfTasks.Source = new TaskHistoryDataSource(this);
            Tasks = new List<string>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            addTaskButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                string taskEntered = taskTextField.Text;
                if(taskEntered == " "){
                    var alert = UIAlertController.Create("Error!", "Enter valid tasks", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
                Tasks.Add(taskEntered);
                taskTextField.Text = " ";
            };

            startButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                while (Tasks.Count == 0)
                {
                    startButton.Enabled = false;
                }
            };

            stopButton.TouchUpInside += (object send, EventArgs e) =>
            {
                while (Tasks.Count == 0)
                {
                    stopButton.Enabled = false;
                }

            };

        }

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