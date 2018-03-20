using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using System.Threading.Tasks;
using System.Text;
using System.Timers;

namespace Pomodoro
{
    public partial class timerController : UIViewController
    {
        // list of tasks entered by the user
        public List<string> Tasks { get; set; }

        static NSString taskHistoryCellID = new NSString("TaskHistoryCell");

        private TaskService taskService;

        public timerController(IntPtr handle) : base(handle)
        {
            Tasks = new List<string>();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        /**
         * Actions to be performed once current view is loaded
         */
        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            var hoursNumberModel = new HourPickerModel();
            hoursSelection.Model = hoursNumberModel;

            var minutesNumberModel = new MinuteSecondPickerModel();
            minutesSelection.Model = minutesNumberModel;

            var secondsNumberModel = new MinuteSecondPickerModel();
            secondsSelection.Model = secondsNumberModel;

            tableOfTasks.RegisterClassForCellReuse(typeof(UITableViewCell), taskHistoryCellID);
            tableOfTasks.Source = new TaskHistoryDataSource(this);
            taskService = TaskService.DefaultService;
            await taskService.InitializeStoreAsync();
            await RefreshAsync();

            //actions by clicking startButton
            startButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                Console.WriteLine("hours: " + hoursNumberModel.SelectedValue);
                Console.WriteLine("minutes: " + minutesNumberModel.SelectedValue);
                Console.WriteLine("seconds: " + secondsNumberModel.SelectedValue);

                //1 Hour = 3,600,000 Milliseconds
                int hoursToMilliseconds = 0;
                if (hoursNumberModel.SelectedValue != 0)
                {
                    hoursToMilliseconds = hoursNumberModel.SelectedValue * 3600000;
                }
                // 1 minute = 60000 ms
                int minutesToMilliseconds = 0;
                if (minutesNumberModel.SelectedValue != 0)
                {
                    minutesToMilliseconds = minutesNumberModel.SelectedValue * 60000;
                }
                // 1 second =  1000 ms
                int secondsToMilliseconds = 0;
                if (secondsNumberModel.SelectedValue != 0)
                {
                    secondsToMilliseconds = secondsNumberModel.SelectedValue * 1000;
                }
                // 2. add to hours
                int totalMS = hoursToMilliseconds + minutesToMilliseconds + secondsToMilliseconds;
                // 3. convert hours to milliseconds
                if (totalMS == 0)
                {
                    var alert = UIAlertController.Create("Invalid Timer!", "Please select again.", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
                else
                {
                    // Timer
                    System.Timers.Timer t = new System.Timers.Timer(totalMS);
                    Console.WriteLine("milliseconds" + totalMS);
                    t.Elapsed += MyTimer_Elapsed;
                    t.Start();
                    Console.ReadLine();
                }
            };

            //actions by clicking stopButton
            stopButton.TouchUpInside += (object send, EventArgs e) =>
            {


            };

        }

        private void MyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Elapsed: {0:HH:mm:ss.fff}", e.SignalTime);
        }

        private async Task RefreshAsync()
        {
            await taskService.RefreshDataAsync();

            tableOfTasks.ReloadData();
        }

        class TaskHistoryDataSource : UITableViewSource
        {
            timerController controller;
            public TaskHistoryDataSource(timerController controller)
            {
                this.controller = controller;
            }
            #region UITableView methods
            public override nint RowsInSection(UITableView tableview, nint section)
            {
                if (controller.taskService == null || controller.taskService.Items == null)
                    return 0;

                return controller.taskService.Items.Count;
            }



            public override nint NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(timerController.taskHistoryCellID);

                int row = indexPath.Row;
                cell.TextLabel.Text = controller.taskService.Items[indexPath.Row].Text;
                return cell;
            }

            public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
            {
                // Customize the Delete button to say "complete"
                return @"complete";
            }

            public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
            {
                // Find the item that is about to be edited
                var item = controller.taskService.Items[indexPath.Row];

                // If the item is complete, then this is just pending upload. Editing is not allowed
                if (item.Complete)
                    return UITableViewCellEditingStyle.None;

                // Otherwise, allow the delete button to appear
                return UITableViewCellEditingStyle.Delete;
            }

            public async override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
            {
                // Find item that was commited for editing (completed)
                var item = controller.taskService.Items[indexPath.Row];

                // Change the appearance to look greyed out until we remove the item
                var label = (UILabel)controller.tableOfTasks.CellAt(indexPath).TextLabel;
                label.TextColor = UIColor.Gray;

                // Ask the todoService to set the item's complete value to YES, and remove the row if successful
                await controller.taskService.CompleteItemAsync(item);

                // Remove the row from the UITableView
                tableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Top);
            }

            #endregion
        }
    }
}