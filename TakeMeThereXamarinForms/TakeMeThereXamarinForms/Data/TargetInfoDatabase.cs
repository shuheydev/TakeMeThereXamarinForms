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
            database.CreateTableAsync<LocationInformation>().Wait();
        }

        public Task<List<LocationInformation>> GetItemsAsync()
        {
            return database.Table<LocationInformation>().OrderByDescending(info=>info.UpdatedAt).ToListAsync();
        }

        //public Task<List<LocationInformation>> GetItemsNotDoneAsync()
        //{
        //    return database.QueryAsync<LocationInformation>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        //}

        public Task<LocationInformation> GetItemAsync(int id)
        {
            return database.Table<LocationInformation>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(LocationInformation item)
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

        public Task<int> DeleteItemAsync(LocationInformation item)
        {
            return database.DeleteAsync(item);
        }

        public Task<int> DeleteAllItemsAsync()
        {
            return database.DeleteAllAsync<LocationInformation>();
        }
    }
}
