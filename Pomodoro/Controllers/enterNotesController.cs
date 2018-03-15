using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Pomodoro
{
    public partial class enterNotesController : UIViewController
    {
        // azure connection for notes table
        private NotesService notesService;

        // set by notesController if note is being edited
        public NotesItem note { get; set; }

        // checker for editing mode
        private bool isInEditingMode = false;

        public enterNotesController(IntPtr handle) : base(handle)
        { }

        /**
         * Initializing objects before storyboard appears
         */
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (note != null)
            {
                isInEditingMode = true;
                notesTextArea.Text = note.Text;
            }
        }

        /**
         * Actions that can occur after storyboard is loaded
         */
        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            notesService = NotesService.DefaultService;
            await notesService.InitializeStoreAsync();
            //actions by clicking done button
            doneNotesButton.TouchUpInside += async (object sender, EventArgs e) =>
            {
                if (string.IsNullOrWhiteSpace(notesTextArea.Text))
                    return;

                if (!isInEditingMode)
                {
                    // new note
                    var newItem = new NotesItem
                    {
                        Text = notesTextArea.Text,
                        Delete = false
                    };
                    await notesService.InsertTodoItemAsync(newItem);
                }
                else
                {
                    // note already exists and has to be updated in azure               
                    note.Text = notesTextArea.Text;
                    await notesService.UpdateNoteAsync(note);
                    isInEditingMode = false;
                }

                notesTextArea.Text = "";
                await notesService.RefreshDataAsync();
            };
        }

        /**
         * Refreshing notes table in notes controller
         */
        public override  void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "doneSegue")
            { 
                var notesCtrl = segue.DestinationViewController as notesController;
                if (notesCtrl != null)
                {
                    // refreshing table before view displays
                     notesCtrl.RefreshAsync();
                }
            }
        }
    }
}