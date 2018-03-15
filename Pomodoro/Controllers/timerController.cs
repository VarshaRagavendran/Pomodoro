using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using System.Threading.Tasks;

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

        /**
         * Actions to be performed once current view is loaded
         */
        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            tableOfTasks.RegisterClassForCellReuse(typeof(UITableViewCell), taskHistoryCellID);
            tableOfTasks.Source = new TaskHistoryDataSource(this);
            taskService = TaskService.DefaultService;
            await taskService.InitializeStoreAsync();
            await RefreshAsync();
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