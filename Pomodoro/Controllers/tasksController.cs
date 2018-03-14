using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using System.Threading.Tasks;

namespace Pomodoro
{
    public partial class tasksController : UIViewController
    {

        static NSString taskHistoryCellID = new NSString("TaskHistoryCell");
        private TaskService taskService;

        public tasksController(IntPtr handle) : base(handle)
        { }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            //register table of tasks and methods that could be called
            tableOfTasks.RegisterClassForCellReuse(typeof(UITableViewCell), taskHistoryCellID);
            tableOfTasks.Source = new TaskHistoryDataSource(this);

            taskService = TaskService.DefaultService;
            await taskService.InitializeStoreAsync();


            await RefreshAsync();

            addTaskButton.TouchUpInside += async (object sender, EventArgs e) =>
            {

                if (string.IsNullOrWhiteSpace(taskTextField.Text))
                    return;

                var newItem = new TaskItem
                {
                    Text = taskTextField.Text,
                    Complete = false
                };

                await taskService.InsertTodoItemAsync(newItem);

                var index = taskService.Items.FindIndex(item => item.Id == newItem.Id);

                tableOfTasks.InsertRows(new[] { NSIndexPath.FromItemSection(index, 0) },
                UITableViewRowAnimation.Top);

                taskTextField.Text = "";
            };

        }

        private async Task RefreshAsync()
        {
            await taskService.RefreshDataAsync();

            tableOfTasks.ReloadData();
        }

        [Export("textFieldShouldReturn:")]
        public virtual bool ShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();
            return true;
        }

        class TaskHistoryDataSource : UITableViewSource
        {
            tasksController controller;
            public TaskHistoryDataSource(tasksController controller)
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
                var cell = tableView.DequeueReusableCell(tasksController.taskHistoryCellID);

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