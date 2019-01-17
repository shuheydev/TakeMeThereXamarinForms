using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TakeMeThereXamarinForms.Models;

namespace TakeMeThereXamarinForms.Data
{
    public class TargetInfoDatabase
    {
        readonly SQLiteAsyncConnection database;

        public TargetInfoDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<TargetInformation>().Wait();
        }

        public Task<List<TargetInformation>> GetItemsAsync()
        {
            return database.Table<TargetInformation>().ToListAsync();
        }

        public Task<List<TargetInformation>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<TargetInformation>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<TargetInformation> GetItemAsync(int id)
        {
            return database.Table<TargetInformation>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(TargetInformation item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(TargetInformation item)
        {
            return database.DeleteAsync(item);
        }

        public Task<int> DeleteAllItemsAsync()
        {
            return database.DeleteAllAsync<TargetInformation>();
        }
    }
}
