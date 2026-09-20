namespace Freightmatic.Application.Users
{
    public record LoginUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
