namespace Freightmatic.Domain.Users;

public interface IUserRepository
{
    Task<User?> CreateUserAsync(User user, string password, string refreshToken);
    Task<bool> UpdateUserAsync(User user);
    Task<User?> GetUserByIdAsync(string id);
    Task<User?> VerifyUser(string username, string password);
    Task<bool> IsExist(string userName, string email, string phoneNumber);
    Task<bool> LogoutUserAsync(string id);
    Task<string> HashPassword(User user, string password);
    Task<bool> VerifyPassword(User user, string password);

    Task<User?> GetUserByUserNameAsync(string userName);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);

    Task<List<User?>> GetAllAsync();
}
