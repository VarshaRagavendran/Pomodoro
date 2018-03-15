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
                    for (int i = 0; i < notesService.Items.Count; i++)
                    {
                        if ((notesService.Items[i].Id).Equals(note.Id))
                        {
                            notesService.Items[i].Text = notesTextArea.Text;
                            note.Text = notesTextArea.Text;
                            await notesService.UpdateNoteAsync(note);
                            break;
                        }
                    }
                    isInEditingMode = false;
                }

                notesTextArea.Text = "";
            };
        }
    }
}