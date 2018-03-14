using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Pomodoro
{

    public partial class notesController : UIViewController
    {
        static NSString notesHistoryCellId = new NSString("NotesHistoryCell");
        public List<string> notes { get; set; }

        public notesController(IntPtr handle) : base(handle)
        {
            notes = new List<string>();
        }

        [Action("UnwindToNotesController:")]
        public void UnwindToNotesController(UIStoryboardSegue segue)
        { }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            listOfAllNotesTable.ReloadData();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            // register tableOfNotes tableview component
            listOfAllNotesTable.RegisterClassForCellReuse(typeof(UITableViewCell), notesHistoryCellId);
            listOfAllNotesTable.Source = new NotesHistoryDataSource(this);
        }

        /**
        * Loading notes data into table view
        */
        class NotesHistoryDataSource : UITableViewSource
        {
            notesController controller;

            public NotesHistoryDataSource(notesController controller)
            {
                this.controller = controller;
            }

            public override nint RowsInSection(UITableView tableView, nint section)
            {
                return controller.notes.Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(notesController.notesHistoryCellId);

                int row = indexPath.Row;
                cell.TextLabel.Text = controller.notes[row];
                return cell;
            }
        }

    }
}