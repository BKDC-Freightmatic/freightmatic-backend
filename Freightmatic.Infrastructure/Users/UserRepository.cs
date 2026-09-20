using Freightmatic.Domain.Users;
using Freightmatic.Infrastructure.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos;
using System.Dynamic;
using User = Freightmatic.Domain.Users.User;

namespace Freightmatic.Infrastructure.Users;

public class UserRepository : IUserRepository
{
    private readonly CosmosDbClient _cosmosDbClient;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserRepository(CosmosDbClient cosmosDbClient, IPasswordHasher<User> passwordHasher)
    {
        _cosmosDbClient = cosmosDbClient;
        _passwordHasher = passwordHasher;
    }

    public async Task<User?> CreateUserAsync(User user, string password, string refreshToken)
    {
        user.Password = _passwordHasher.HashPassword(user, password);
        user.RefreshToken = refreshToken;

        var response = await _cosmosDbClient.CreateItemAsync(user);

        if (response.StatusCode != System.Net.HttpStatusCode.Created)
            return null;

        return user;
    }

    public async Task<string> HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public async Task<bool> VerifyPassword(User user, string password)
    {
        return _passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Success;
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        var query = new QueryDefinition("SELECT * FROM users WHERE users.UserName = @UserName").WithParameter("@UserName", userName);
        var items = await _cosmosDbClient.GetItemsAsync<User>(query);
        return items.SingleOrDefault();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var query = new QueryDefinition("SELECT * FROM users WHERE users.Email = @Email").WithParameter("@Email", email);
        var items = await _cosmosDbClient.GetItemsAsync<User>(query);
        return items.SingleOrDefault();
    }

    public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        var query = new QueryDefinition("SELECT * FROM users WHERE users.PhoneNumber = @PhoneNumber").WithParameter("@PhoneNumber", phoneNumber);
        var items = await _cosmosDbClient.GetItemsAsync<User>(query);
        return items.SingleOrDefault();
    }

    public async Task<User?> VerifyUser(string username, string password)
    {
        User? user = await GetUserByUserNameAsync(username);

        if (user == null)
            user = await GetUserByEmailAsync(username);

        if (user == null)
            user = await GetUserByPhoneNumberAsync(username);

        if (user == null)
            return null;

        PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        if (verificationResult != PasswordVerificationResult.Success)
            return null;

        return user;
    }

    public async Task<bool> IsExist(string userName, string email, string phoneNumber)
    {
        User? user = await GetUserByUserNameAsync(userName);

        if (user == null)
            user = await GetUserByEmailAsync(email);

        if (user == null)
            user = await GetUserByPhoneNumberAsync(phoneNumber);

        return user != null;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        var response = await _cosmosDbClient.UpdateItemAsync(user);

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return false;

        return true;
    }

    public async Task<bool> LogoutUserAsync(string id)
    {
        User user = await _cosmosDbClient.GetItemByIdAsync<User>(id);
        if (user == null)
            return false;

        user.RefreshToken = null;
        bool isSuccess = await UpdateUserAsync(user);
        return isSuccess;
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        User user = await _cosmosDbClient.GetItemByIdAsync<User>(id);
        return user;
    }

    public async Task<List<User?>> GetAllAsync()
    {
        var query = new QueryDefinition("SELECT * FROM users");
        List<User?> items = await _cosmosDbClient.GetItemsAsync<User?>(query);
        return items;
    }
}
