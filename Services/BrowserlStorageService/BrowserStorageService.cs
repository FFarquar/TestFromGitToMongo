using Blazored.LocalStorage;

namespace TestFromGitToMongo.Services.BrowserStorageService
{
    public class BrowserStorageService : IBrowserStorageService
    {
        private readonly ILocalStorageService _localStorageService;

        public BrowserStorageService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task AddItemToStorage(string key, string value)
        {
            await _localStorageService.SetItemAsStringAsync(key, value);

        }

        public async Task<string> GetItemFromStorage(string key)
        {
            var res = await _localStorageService.GetItemAsStringAsync(key);

            return res;
        }

        public async Task RemoveItemFromStorage(string key)
        {
            await _localStorageService.RemoveItemAsync(key);
        }
    }
}
