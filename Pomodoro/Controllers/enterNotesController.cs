using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Pomodoro
{
    public partial class enterNotesController : UIViewController
    {
        private NotesService notesService;

        public enterNotesController(IntPtr handle) : base(handle)
        {}

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
                
                var newItem = new NotesItem
                {
                    Text = notesTextArea.Text,
                    Delete = false
                };

                await notesService.InsertTodoItemAsync(newItem);
                notesTextArea.Text = "";
            };
        }
    }
}