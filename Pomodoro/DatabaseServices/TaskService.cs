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
    public class TaskService
    {
        static TaskService instance = new TaskService();

        const string applicationURL = @"https://pomodorotracker.azurewebsites.net";

        private MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        const string localDbPath    = "localstore.db";

        private IMobileServiceSyncTable<ToDoItem> todoTable;
#else
        private IMobileServiceTable<TaskItem> todoTable;
#endif

        private TaskService()
        {
            CurrentPlatform.Init();

            // Initialize the client with the mobile app backend URL.
            client = new MobileServiceClient(applicationURL);

#if OFFLINE_SYNC_ENABLED
            // Initialize the store
            InitializeStoreAsync().Wait();
            // Create an MSTable instance to allow us to work with the TodoItem table
            todoTable = client.GetSyncTable<ToDoItem>();
#else
            todoTable = client.GetTable<TaskItem>();
#endif
        }

        public static TaskService DefaultService
        {
            get
            {
                return instance;
            }
        }

        public List<TaskItem> Items { get; private set; }

        public async Task InitializeStoreAsync()
        {
#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore(localDbPath);
            store.DefineTable<ToDoItem>();

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

        public async Task<List<TaskItem>> RefreshDataAsync()
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                // Update the local store
                await SyncAsync(pullData: true);
#endif

                // This code refreshes the entries in the list view by querying the local TodoItems table.
                // The query excludes completed TodoItems
                Items = await todoTable
                        .Where(todoItem => todoItem.Complete == false).ToListAsync();

            }
            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return null;
            }

            return Items;
        }

        public async Task InsertTodoItemAsync(TaskItem todoItem)
        {
            try
            {
                await todoTable.InsertAsync(todoItem); // Insert a new TodoItem into the local database.
#if OFFLINE_SYNC_ENABLED
                await SyncAsync(); // Send changes to the mobile app backend.
#endif

                Items.Add(todoItem);

            }
            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
        }

        public async Task CompleteItemAsync(TaskItem item)
        {
            try
            {
                item.Complete = true;
                await todoTable.UpdateAsync(item); // Update todo item in the local database
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

