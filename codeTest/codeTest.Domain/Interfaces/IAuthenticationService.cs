namespace RapidPay.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> SignInAsync(User user);
        Task<string> CalculateHashAsync(string input);
    }
}
