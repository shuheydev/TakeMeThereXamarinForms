using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using TakeMeThereXamarinForms.Models;

namespace TakeMeThereXamarinForms.Data
{
    public class TargetInfoDatabase
    {
        private readonly SQLiteAsyncConnection database;

        public TargetInfoDatabase(string dbPath)
        {
            //database = new SQLiteAsyncConnection(dbPath);
            //database.CreateTableAsync<Place>().Wait();
        }

        public Task<List<Place>> GetItemsAsync()
        {
            return database.Table<Place>().ToListAsync();
        }

        //public Task<List<LocationInformation>> GetItemsNotDoneAsync()
        //{
        //    return database.QueryAsync<LocationInformation>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        //}

        public Task<Place> GetItemAsync(int id)
        {
            return database.Table<Place>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Place item)
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

        public Task<int> DeleteItemAsync(Place item)
        {
            return database.DeleteAsync(item);
        }

        public Task<int> DeleteAllItemsAsync()
        {
            return database.DeleteAllAsync<Place>();
        }
    }
}