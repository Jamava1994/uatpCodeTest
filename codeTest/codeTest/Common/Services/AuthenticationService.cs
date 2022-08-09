
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RapidPay.Domain;
using RapidPay.Domain.Interfaces;
using RapidPay.Infrastructure.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RapidPay.Application.Common.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly RapidPayDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationService(RapidPayDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> SignInAsync(User user)
        {
            var passwordHash = await CalculateHashAsync(user.Password);

            var userLoggedIn = await _context.Users
                .AsNoTracking()
                .AnyAsync(x => x.Username == user.Username && x.Password == passwordHash)
                .ConfigureAwait(false);

            if (!userLoggedIn)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
             new Claim(ClaimTypes.Name, user.Username)
              }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<string> CalculateHashAsync(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                Stream inputBytes = new MemoryStream(Encoding.ASCII.GetBytes(input));
                byte[] hashBytes = await md5.ComputeHashAsync(inputBytes);
                return Convert.ToHexString(hashBytes);
            }
        }
    }
}
