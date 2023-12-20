using TestFromGitToMongo.Models;

namespace TestFromGitToMongo.Services.AuthService
{
    public interface IAuthService
    {
     //   public string jwttoken { get; set; }
        Task<ServiceResponse<int>> Register(UserRegister request);
        Task<ServiceResponse<int>> TestCall(UserRegister request);
        Task<ServiceResponse<string>> Login(UserLogin request);
        Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request);
        Task<ServiceResponse<bool>> ChangeUserToAdmin();   //a test method to see if can use example route on node
    }
}
