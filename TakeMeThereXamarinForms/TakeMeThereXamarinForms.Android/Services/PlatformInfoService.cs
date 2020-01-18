using System.IO;
using TakeMeThereXamarinForms.Services;

namespace TakeMeThereXamarinForms.Droid.Services
{
    public class PlatformInfoService : IPlatformInfoService
    {
        private string _dbFileName = "AppDb.db";

        public string GetUserDataFolderPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
        }

        public string GetSqliteDbPath()
        {
            return Path.Combine(GetUserDataFolderPath(), _dbFileName);
        }
    }
}