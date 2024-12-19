namespace KanbApp.Services
{
    public interface IAuthService
    {
        Task<bool> IsUserAuthenticated();
    }

    public class AuthService : IAuthService
    {
        private readonly UserService _userService;

        public AuthService(UserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var user = await _userService.GetLoggedInUserAsync();
            return user != null;
        }
    }
}
