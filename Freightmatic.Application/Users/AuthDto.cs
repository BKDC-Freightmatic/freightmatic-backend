namespace Freightmatic.Application.Users
{
    public class AuthDto
    {
        public UserDto User { get; set; }
        public Token Tokens { get; set; }
    }

    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}