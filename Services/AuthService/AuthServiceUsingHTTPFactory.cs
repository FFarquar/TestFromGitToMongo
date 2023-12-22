namespace TestFromGitToMongo.Services.AuthService
{
    public class AuthServiceUsingHTTPFactory : IAuthService
    {
        private readonly BikeAPIClient _client;
        private readonly IBrowserStorageService _localStorService;

        public AuthServiceUsingHTTPFactory(BikeAPIClient bikesClient, IBrowserStorageService localStorService)
        {
            _client = bikesClient;
            _localStorService = localStorService;
        }

        public Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> ChangeUserToAdmin()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<string>> Login(UserLogin request)
        {
            return await _client.Login(request);
        }

        public Task<ServiceResponse<int>> Register(UserRegister request)
        {
            throw new NotImplementedException();
        }


        public Task<ServiceResponse<int>> TestCall(UserRegister request)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<bool>> TestAdminRoute()
        {
            return  await _client.TestTheAuthorizedAdminRoute();

        }
    }
}
