namespace TakeMeThereXamarinForms.Services
{
    public interface IPlatformInfoService
    {
        string GetUserDataFolderPath();
        string GetSqliteDbPath();
    }
}