using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using System.Threading.Tasks;
using System.Text;

namespace Pomodoro
{
    public partial class notesController : UIViewController
    {
        static NSString notesHistoryCellId = new NSString("NotesHistoryCell");
        private NotesService notesService;

        public notesController(IntPtr handle) : base(handle)
        { }

        /**
         * Unwinding when home button is clicked
         */
        [Action("UnwindToNotesController:")]
        public void UnwindToNotesController(UIStoryboardSegue segue)
        { }

        /**
         * Refreshing data / loading data before storyboard appears
         */
        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            // register tableOfNotes tableview component
            listOfAllNotesTable.RegisterClassForCellReuse(typeof(UITableViewCell), notesHistoryCellId);
            listOfAllNotesTable.Source = new NotesHistoryDataSource(this);
            notesService = NotesService.DefaultService;
            await notesService.InitializeStoreAsync();
            // pull the latest data
            await RefreshAsync();
           
        }

        /**
         * Refreshes entries in the list view by querying the local notes table
         */
        public async Task RefreshAsync()
        {
            await notesService.RefreshDataAsync();
            listOfAllNotesTable.ReloadData();
        }

        /**
         * Sending selected NotesItem to EnterNotescontroller for editing
         */
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "notesEditor")
            { // set in Storyboard
                NSIndexPath indexPath = (NSIndexPath)sender;
                var enterNotesCtrl = segue.DestinationViewController as enterNotesController;
                if (enterNotesCtrl != null)
                {
                    enterNotesCtrl.note = notesService.Items[indexPath.Row];
                }
            }

        }

        /**
        * Loading notes data into table view and methods that can be performed
        */
        class NotesHistoryDataSource : UITableViewSource
        {
            private int numberOfCharactersToBeDisplayed = 30;

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
                string itemText = controller.notesService.Items[indexPath.Row].Text;
                if (itemText.Length < numberOfCharactersToBeDisplayed)
                    cell.TextLabel.Text = itemText;
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(itemText.Substring(0, numberOfCharactersToBeDisplayed));
                    sb.Append("...");
                    cell.TextLabel.Text = sb.ToString();
                }

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

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                controller.PerformSegue("notesEditor", indexPath); // pass indexPath as sender
            }
            #endregion
        }

    }
}