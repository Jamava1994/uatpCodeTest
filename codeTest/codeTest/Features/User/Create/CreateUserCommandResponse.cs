namespace RapidPay.Application.Features.User.Create
{
    public class CreateUserCommandResponse
    {
        public string Username { get; set; }
        public string Password { get; } = "***********";
    }
}
