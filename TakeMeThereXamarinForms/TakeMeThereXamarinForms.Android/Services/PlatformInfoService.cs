using System.IO;
using TakeMeThereXamarinForms.Services;

namespace TakeMeThereXamarinForms.Droid.Services
{
    public class PlatformInfoService : IPlatformInfoService
    {
        public string GetUserDataFolderPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "placesDb.db");
        }
    }
}