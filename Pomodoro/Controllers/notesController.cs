using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using System.Threading.Tasks;

namespace Pomodoro
{
    public partial class notesController : UIViewController
    {
        static NSString notesHistoryCellId = new NSString("NotesHistoryCell");
        private NotesService notesService;

        public notesController(IntPtr handle) : base(handle)
        { }

        [Action("UnwindToNotesController:")]
        public void UnwindToNotesController(UIStoryboardSegue segue)
        { }

        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            notesService = NotesService.DefaultService;
            await notesService.InitializeStoreAsync();
            await RefreshAsync();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            // register tableOfNotes tableview component
            listOfAllNotesTable.RegisterClassForCellReuse(typeof(UITableViewCell), notesHistoryCellId);
            listOfAllNotesTable.Source = new NotesHistoryDataSource(this);
        }

        private async Task RefreshAsync()
        {
            await notesService.RefreshDataAsync();
            listOfAllNotesTable.ReloadData();
        }


        /**
        * Loading notes data into table view and methods that can be performed
        */
        class NotesHistoryDataSource : UITableViewSource
        {
            notesController controller;
            public NotesHistoryDataSource(notesController controller)
            {
                this.controller = controller;
            }

            #region UITableView methods
            public override nint RowsInSection(UITableView tableview, nint section)
            {
                if (controller.notesService == null || controller.notesService.Items == null)
                    return 0;

                return controller.notesService.Items.Count;
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(notesController.notesHistoryCellId);
                int row = indexPath.Row;
                cell.TextLabel.Text = controller.notesService.Items[indexPath.Row].Text;
                return cell;
            }

            public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
            {
                // Find the item that is about to be edited
                var item = controller.notesService.Items[indexPath.Row];

                // If the item is to be deleted, then this is just pending upload. Editing is not allowed
                if (item.Delete)
                    return UITableViewCellEditingStyle.None;

                // Otherwise, allow the delete button to appear
                return UITableViewCellEditingStyle.Delete;
            }

            public async override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
            {
                // Find item that was commited for editing
                var item = controller.notesService.Items[indexPath.Row];

                // Change the appearance to look greyed out until we remove the item
                var label = (UILabel)controller.listOfAllNotesTable.CellAt(indexPath).TextLabel;
                label.TextColor = UIColor.Gray;

                // Ask the notesService to set the item's delete value to YES, and remove the row if successful
                await controller.notesService.CompleteItemAsync(item);

                // Remove the row from the UITableView
                tableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Top);
            }



            #endregion
        }

    }
}