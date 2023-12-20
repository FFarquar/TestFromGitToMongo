namespace TestFromGitToMongo.Services.BrowserStorageService
{
    public interface IBrowserStorageService
    {
        Task AddItemToStorage(string key, string value);
        Task RemoveItemFromStorage(string key);
        Task<string> GetItemFromStorage(string key);
    }
}
