using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;  // offline sync
using Microsoft.WindowsAzure.MobileServices.Sync;         // offline sync
#endif

namespace Pomodoro
{
    public class NotesService
    {
        static NotesService instance = new NotesService();

        const string applicationURL = @"https://pomodorotracker.azurewebsites.net";

        private MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        const string localDbPath    = "localstore.db";

        private IMobileServiceSyncTable<NotesItem> notesTable;
#else
        private IMobileServiceTable<NotesItem> notesTable;
#endif

        private NotesService()
        {
            CurrentPlatform.Init();

            // Initialize the client with the mobile app backend URL.
            client = new MobileServiceClient(applicationURL);

#if OFFLINE_SYNC_ENABLED
            // Initialize the store
            InitializeStoreAsync().Wait();
            // Create an MSTable instance to allow us to work with the TodoItem table
            notesTable = client.GetSyncTable<NotesItem>();
#else
            notesTable = client.GetTable<NotesItem>();
#endif
        }

        public static NotesService DefaultService
        {
            get
            {
                return instance;
            }
        }

        public List<NotesItem> Items { get; private set; }

        public async Task InitializeStoreAsync()
        {
#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore(localDbPath);
            store.DefineTable<NotesItem>();

            await client.SyncContext.InitializeAsync(store);
#endif
        }

        public async Task SyncAsync(bool pullData = false)
        {
#if OFFLINE_SYNC_ENABLED
            try
            {
                await client.SyncContext.PushAsync();

                if (pullData) {
                    await todoTable.PullAsync("allTodoItems", todoTable.CreateQuery()); // query ID is used for incremental sync
                }
            }

            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"Sync Failed: {0}", e.Message);
            }
#endif
        }

        /**
         * Refreshes the entries in the list view by querying the local notes table.
         * Excludes Deleted items
         */
        public async Task<List<NotesItem>> RefreshDataAsync()
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                // Update the local store
                await SyncAsync(pullData: true);
#endif
                Items = await notesTable
        .Where(notesItem => notesItem.Delete == false).ToListAsync();

            }
            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return null;
            }

            return Items;
        }

        /**
         * Inserts a new note item into azure
         */
        public async Task InsertTodoItemAsync(NotesItem notesItem)
        {
            try
            {
                await notesTable.InsertAsync(notesItem);
#if OFFLINE_SYNC_ENABLED
                await SyncAsync(); // Send changes to the mobile app backend.
#endif

                Items.Add(notesItem);

            }
            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
        }

        /**
         * Updates an existing note item in azure
         */
        public async Task UpdateNoteAsync(NotesItem notesItem)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if ((Items[i].Id).Equals(notesItem.Id))
                {
                    Items[i].Text = notesItem.Text;
                }
            }
            await notesTable.UpdateAsync(notesItem);
        }

        /**
         * Deleting an existing note item in azure
         */
        public async Task CompleteItemAsync(NotesItem item)
        {
            try
            {
                item.Delete = true;
                await notesTable.UpdateAsync(item);
#if OFFLINE_SYNC_ENABLED
                await SyncAsync(); // Send changes to the mobile app backend.
#endif

                Items.Remove(item);

            }
            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
        }
    }
}

